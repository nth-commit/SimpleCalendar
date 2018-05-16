using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimpleCalendar.Utility.DependencyInjection
{
    public class ValidatableServiceCollection : IValidatableServiceCollection
    {
        private readonly IServiceCollection _innerServiceCollection;
        private readonly List<Type> _requiredTypes = new List<Type>();

        public ValidatableServiceCollection(IServiceCollection innerSeviceCollection = null)
        {
            _innerServiceCollection = innerSeviceCollection ?? new ServiceCollection();
        }

        public void AddRequirement(Type type)
        {
            _requiredTypes.Add(type);
        }

        public void ValidateRequirements()
        {
            var serviceProvider = _innerServiceCollection.BuildServiceProvider();
            foreach (var type in _requiredTypes)
            {
                var service = serviceProvider.GetService(type);
                if (service == null)
                {
                    throw new Exception($"Service is required: {type.FullName}");
                }
            }
        }


        #region IServiceCollection

        public ServiceDescriptor this[int index] { get => _innerServiceCollection[index]; set => _innerServiceCollection[index] = value; }

        public int Count => _innerServiceCollection.Count;

        public bool IsReadOnly => _innerServiceCollection.IsReadOnly;

        public void Add(ServiceDescriptor item)
        {
            _innerServiceCollection.Add(item);
        }

        public void Clear()
        {
            _innerServiceCollection.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return _innerServiceCollection.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _innerServiceCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _innerServiceCollection.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            return _innerServiceCollection.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _innerServiceCollection.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            return _innerServiceCollection.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _innerServiceCollection.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerServiceCollection.GetEnumerator();
        }

        #endregion
    }
}
