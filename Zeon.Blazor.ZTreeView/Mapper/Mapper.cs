
namespace Zeon.Blazor.ZTreeView
{
    public class Mapper<Source>
    {
        private readonly Dictionary<string, string>? _fieldsMapSettings;
        public Mapper(Dictionary<string, string>? fieldsMapSettings = null)
        {
            _fieldsMapSettings = fieldsMapSettings;
        }

        public List<Model> CreateMap(IEnumerable<Source> sourceList)
        {
            var mapList = new List<Model>();
            foreach (var source in sourceList)
            {
                var sourceType = source!.GetType();
                var newInstance = (Model)Activator.CreateInstance(typeof(Model))!;
                var newInstanceType = newInstance.GetType();
                foreach (var newInstancePI in newInstanceType.GetProperties())
                {
                    foreach (var sourcePI in sourceType.GetProperties())
                    {
                        if ((newInstancePI.Name == sourcePI.Name || FieldIsMappedByAnotherName(sourcePI.Name, newInstancePI.Name)) && newInstancePI.PropertyType == sourcePI.PropertyType && newInstancePI.CanWrite)
                        {
                            newInstancePI.SetValue(newInstance, sourcePI.GetValue(source));
                            break;
                        }
                    }
                }
                mapList.Add(newInstance);
            }
            return mapList;
        }

        private bool FieldIsMappedByAnotherName(string sourceFieldName, string destinationFieldName)
        {
            return _fieldsMapSettings?.Any(q => q.Key == sourceFieldName).ToString() == destinationFieldName;
        }

    }
}
