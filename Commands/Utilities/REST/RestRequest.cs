using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Utilities.REST
{
    public class RestRequest
    {
        private string _root;
        private List<string> _expands;
        private List<string> _selects;
        private string _filter;
        private ClientContext _context;
        private uint? _top = null;

        public RestRequest(ClientContext context)
        {
            _context = context;
            _expands = new List<string>();
            _selects = new List<string>();
            _filter = "";
        }

        public RestRequest(ClientContext context, string root)
        {
            _context = context;
            _root = root;
            _expands = new List<string>();
            _selects = new List<string>();
        }

        public RestRequest Filter(string filter)
        {
            _filter = filter;
            return this;
        }

        public RestRequest Root(string root)
        {
            _root = root;
            return this;
        }

        public RestRequest Top(uint count)
        {
            _top = count;
            return this;
        }

        public RestRequest Expand(params string[] expand)
        {
            _expands.AddRange(expand);
            return this;
        }

        public RestRequest Select(params string[] select)
        {
            _selects.AddRange(select);
            return this;
        }

        public T Get<T>()
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            return RestHelper.ExecuteGetRequest<T>(_context, _root, select, _filter, expands, _top);
        }


        public string Get()
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            return RestHelper.ExecuteGetRequest(_context, _root, select, _filter, expands, _top);
        }

        public T Post<T>(string content = null, string contentType = "application/json;odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            return RestHelper.ExecutePostRequest<T>(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public void Post(string content = null, string contentType = "application/json;odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            RestHelper.ExecutePostRequest(_context, _root, content, select, _filter, expands, contentType: contentType);
        }
        public void Post(string metadataType, Dictionary<string, object> properties, string contentType = "application/json; odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            properties["__metadata"] = new MetadataType(metadataType);
            var content = JsonSerializer.Serialize(properties);
            RestHelper.ExecutePostRequest(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public T Post<T>(string metadataType, Dictionary<string, object> properties, string contentType = "application/json; odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            properties["__metadata"] = new MetadataType(metadataType);
            var payload = new Dictionary<string, object>() {
                { "parameters", properties}
            };
            var content = JsonSerializer.Serialize(payload);

            return RestHelper.ExecutePostRequest<T>(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public T Put<T>(string content = null, string contentType = null)
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            return RestHelper.ExecutePutRequest<T>(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public void Put(string content = null, string contentType = null)
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            RestHelper.ExecutePutRequest(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public T Merge<T>(string content = null, string contentType = null)
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            return RestHelper.ExecuteMergeRequest<T>(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public T Merge<T>(string metadataType, Dictionary<string, object> properties, string contentType = "application/json;odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            properties["__metadata"] = new MetadataType(metadataType);
            var content = JsonSerializer.Serialize(properties);
            return RestHelper.ExecuteMergeRequest<T>(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public void Merge(string metadataType, Dictionary<string, object> properties, string contentType = "application/json;odata=verbose")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            properties["__metadata"] = new MetadataType(metadataType);
            var content = JsonSerializer.Serialize(properties);
            RestHelper.ExecuteMergeRequest(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public void Merge(string content = null, string contentType = null)
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            RestHelper.ExecuteMergeRequest(_context, _root, content, select, _filter, expands, contentType: contentType);
        }

        public void Delete(string content = null, string contentType = "application/json;odata=version")
        {
            var select = _selects.Any() ? string.Join(",", _selects) : null;
            var expands = _expands.Any() ? string.Join(",", _expands) : null;
            RestHelper.ExecuteDeleteRequest(_context, _root, content, select, _filter);
        }
    }
}
