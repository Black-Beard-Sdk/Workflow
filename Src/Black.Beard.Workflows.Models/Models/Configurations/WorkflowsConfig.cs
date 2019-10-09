using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bb.Workflows.Models.Configurations
{
    public class WorkflowsConfig
    {

        public WorkflowsConfig SetVersionSelector(string workflowName, int version)
        {
            if (!_defaultSelectors.TryGetValue(workflowName, out HashSet<int> versions))
                _defaultSelectors.Add(workflowName, (versions = new HashSet<int>()));
            versions.Add(version);
            return this;
        }

        public void AddDocument(WorkflowConfig config)
        {

            lock (_lock)
            {

                _configs.Add(config);

                _configLookups = _configs.ToLookup(c => c.Name);

                _configDic = _configs.ToDictionary(c => (c.Name, c.Version));

                _subConfigs = new List<WorkflowConfig>();
                foreach (WorkflowConfig _config in this._configs)
                    // if many versions of kind of workflow exist you can only evaluate a specific version
                    if (!_defaultSelectors.ContainsKey(_config.Name) || (_defaultSelectors.TryGetValue(_config.Name, out HashSet<int> versions) && versions.Contains(_config.Version)))
                        _subConfigs.Add(_config);

            }

        }

        public IEnumerable<WorkflowConfig> Get(IncomingEvent @event)
        {

            if (this._configs.Count == 0)
                Trace.WriteLine(new { Message = "no state configuration loaded" });

            foreach (WorkflowConfig config in this._subConfigs)
                if (config.EvaluateFilter(@event))
                    yield return config;

            if (this._subConfigs.Count == 0)
                Trace.WriteLine(new { Message = "no state configuration loaded" });

        }

        public IEnumerable<WorkflowConfig> Get(string workflowName)
        {

            if (this._configs.Count == 0)
                Trace.WriteLine(new { Message = "no state configuration loaded" });

            return _configLookups[workflowName];

        }

        public WorkflowConfig Get(string workflowName, int version)
        {

            if (this._configs.Count == 0)
                Trace.WriteLine(new { Message = "no state configuration loaded" });

            return _configDic[(workflowName, version)];

        }

        private readonly Dictionary<string, HashSet<int>> _defaultSelectors = new Dictionary<string, HashSet<int>>();
        private readonly List<WorkflowConfig> _configs = new List<WorkflowConfig>();
        private List<WorkflowConfig> _subConfigs = new List<WorkflowConfig>();
        private ILookup<string, WorkflowConfig> _configLookups;
        private Dictionary<(string Name, int Version), WorkflowConfig> _configDic;
        private volatile object _lock = new object();

    }

}
