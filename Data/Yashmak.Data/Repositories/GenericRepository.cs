namespace Yashmak.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    using Yashmak.Common.Extentions;
    using Yashmak.Data.Common.Repository;

    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        public GenericRepository(IYashmakDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        protected IYashmakDbContext Context { get; set; }

        protected IDbSet<T> DbSet { get; set; }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public virtual IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(object id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        /// <summary>
        ///     This method updates database values by using expression. It works with both anonymous and class objects.
        ///     It is used in one of the following ways:
        ///     1. .UpdateValues(x => new Type { Id = ..., Property = ..., AnotherProperty = ... })
        ///     2. .UpdateValues(x => new { Id = ..., Property = ..., AnotherProperty = ... })
        /// </summary>
        /// <param name="entity">Expression for the updated entity</param>
        public virtual void UpdateValues(Expression<Func<T, object>> entity)
        {
            // compile the expression to delegate and invoke it
            object compiledExpression = entity.Compile()(null);

            // cast the result of invokation to T
            T updatedEntity = compiledExpression is T ? compiledExpression as T : compiledExpression.CastTo<T>();

            // attach the entry if missing in ObjectStateManager
            var entry = this.Context.Entry(updatedEntity);

            if (entry.State == EntityState.Detached)
            {
                try
                {
                    this.DbSet.Attach(updatedEntity);
                }
                catch
                {
                    var key = GetPrimaryKey(entry);
                    entry = this.Context.Entry(this.DbSet.Find(key));
                    entry.CurrentValues.SetValues(updatedEntity);
                }
            }

            // get current database values of the entity
            var values = entry.GetDatabaseValues();
            if (values == null)
            {
                throw new InvalidOperationException(
                    "Object does not exists in ObjectStateDictionary. Entity Key|Id should be provided or valid.");
            }

            // select the updated members as property names
            IEnumerable<string> members;
            if (compiledExpression is T)
            {
                members = ((MemberInitExpression)entity.Body).Bindings.Select(b => b.Member.Name);
            }
            else
            {
                members = ((NewExpression)entity.Body).Members.Select(m => m.Name);
            }

            // select all not mapped properties and set value
            typeof(T).GetProperties()
                .Where(pr => !pr.GetCustomAttributes(typeof(NotMappedAttribute), true).Any())
                .ForEach(
                    prop =>
                        {
                            if (members.Contains(prop.Name))
                            {
                                // if a member is updated set its state to modified
                                entry.Property(prop.Name).IsModified = true;
                            }
                            else
                            {
                                // otherwise set the existing database value
                                var value = values.GetValue<object>(prop.Name);
                                prop.SetValue(entry.Entity, value);
                            }
                        });
        }

        private static int GetPrimaryKey(DbEntityEntry entry)
        {
            var myObject = entry.Entity;

            var property =
                myObject.GetType()
                    .GetProperties()
                    .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));

            return (int)property.GetValue(myObject, null);
        }
    }
}