using System.Collections.Generic;
namespace Core.Shared
{
    public class ModelList
    {
        public string strObject { get; set; }
        public List<ModelData> data { get; set; }
    }

    public class ModelData
    {
        public string id { get; set; }
        public string strObject { get; set; }
        public long created { get; set; }
        public string owned_by { get; set; }
        public List<ModelPermission> permission { get; set; }
        public string root { get; set; }
        public object parent { get; set; }
    }

    public class ModelPermission
    {
        public string id { get; set; }
        public string strObject { get; set; }
        public long created { get; set; }
        public bool allow_create_engine { get; set; }
        public bool allow_sampling { get; set; }
        public bool allow_logprobs { get; set; }
        public bool allow_search_indices { get; set; }
        public bool allow_view { get; set; }
        public bool allow_fine_tuning { get; set; }
        public string organization { get; set; }
        public object group { get; set; }
        public bool is_blocking { get; set; }
    }
}
