using Bb.Dao;
using Bb.Workflows;
using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Black.Beard.Workflows.Outputs.Sql
{

    public class WorkflowStoreSql : IDisposable
    {

        public WorkflowStoreSql(SqlManager manager, Func<DynObject, string> func)
        {
            this._func = func;
            this._manager = manager;
        }

        public void Save(Workflow workflow)
        {

            var bytes = Encoding.UTF8.GetBytes(_func(workflow.ExtendedDatas));
            var txt = Convert.ToBase64String(bytes);

            if (workflow.Change == ChangeEnum.New)
                _manager.Update("INSERT INTO [Workflows] ([Uuid], [ExternalId], [ExternalIdCrc], [CreationDate], [LastUpdateDate], [WorkflowName], [WorkflowVersion], [Closed], [Concurency], [Datas]) VALUES (@Uuid, @externalId, @externalIdCrc, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @workflowName, @workflowVersion, 0, @concurency, @datas)"
                , _manager.CreateParameter("Uuid", System.Data.DbType.Guid, workflow.Uuid)
                , _manager.CreateParameter("externalId", System.Data.DbType.Guid, workflow.ExternalId)
                , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, Crc32.Calculate(workflow.ExternalId))
                , _manager.CreateParameter("workflowName", System.Data.DbType.String, workflow.WorkflowName)
                , _manager.CreateParameter("workflowVersion", System.Data.DbType.Int16, workflow.Version)
                , _manager.CreateParameter("concurency", System.Data.DbType.Int32, workflow.Concurency)
                , _manager.CreateParameter("datas", System.Data.DbType.String, txt)
                );

            int index = 0;
            foreach (var @event in workflow.Events)
            {

                index++;

                if (@event.Change == ChangeEnum.New)
                    _manager.Update("INSERT INTO [WorkflowEvents] ([Uuid], [WorkflowUuid], [Name], [order], [ExternalIdCrc], [Closed], [CreationDate], [EventDate], [FromState], [ToState], [ToStateCrc], [Datas]) VALUES (@uuid, @workflowUuid, @name, @externalIdCrc, 0, CURRENT_TIMESTAMP, @eventDate, @fromState, @toState, @toStateCrc, @datas)"
                        , _manager.CreateParameter("Uuid", System.Data.DbType.Guid, @event.Uuid)
                        , _manager.CreateParameter("workflowUuid", System.Data.DbType.Guid, workflow.Uuid)
                        , _manager.CreateParameter("name", System.Data.DbType.String, @event.Name)
                        , _manager.CreateParameter("order", System.Data.DbType.Int32, index)
                        , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, Crc32.Calculate(workflow.ExternalId))
                        , _manager.CreateParameter("eventDate", System.Data.DbType.DateTimeOffset, @event.EventDate)
                        , _manager.CreateParameter("fromState", System.Data.DbType.String, @event.FromState)
                        , _manager.CreateParameter("toState", System.Data.DbType.String, @event.ToState)
                        , _manager.CreateParameter("toStateCrc", System.Data.DbType.UInt32, Crc32.Calculate(@event.ToState))
                        , _manager.CreateParameter("datas", System.Data.DbType.String, txt)
                        );


                int index2 = 0;

                foreach (var action in @event.Actions)
                {
                    index2++;
                    if (action.Change == ChangeEnum.New)
                        _manager.Update("INSERT INTO [WorkflowActions] ([Uuid], [EventUuid], [Name], [oder], [ExternalIdCrc], [Closed], [ExecuteMessage], [ResultExecuteMessage], [CancelExecuteMessage], [ResultCancelExecuteMessage]) VALUES (@uuid, @eventUuid, @name, @externalIdCrc, 0, @executeMessage, @resultExecuteMessage, @cancelExecuteMessage, @resultCancelExecuteMessage)"
                            , _manager.CreateParameter("uuid", System.Data.DbType.Guid, action.Uuid)
                            , _manager.CreateParameter("workflowUuid", System.Data.DbType.Guid, @event.Uuid)
                            , _manager.CreateParameter("name", System.Data.DbType.String, action.Name)
                            , _manager.CreateParameter("order", System.Data.DbType.Int32, index2)
                            , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, Crc32.Calculate(workflow.ExternalId))
                            , _manager.CreateParameter("executeMessage", System.Data.DbType.String, action.ExecuteMessage)
                            , _manager.CreateParameter("resultExecuteMessage", System.Data.DbType.String, action.ResultMessage)
                            , _manager.CreateParameter("cancelExecuteMessage", System.Data.DbType.String, action.CancelMessage)
                            , _manager.CreateParameter("resultCancelExecuteMessage", System.Data.DbType.String, action.ResultCancelMessage)
                            );


                    else if (action.Change == ChangeEnum.Updated)
                        _manager.Update("UPDATE [WorkflowActions] SET [ExecuteMessage] = @executeMessage, [ResultExecuteMessage] = @resultExecuteMessage, [CancelExecuteMessage] = @cancelExecuteMessage, [ResultCancelExecuteMessage] =@resultCancelExecuteMessage WHERE [Uuid] = @uuid"
                            , _manager.CreateParameter("uuid", System.Data.DbType.Guid, action.Uuid)
                            , _manager.CreateParameter("executeMessage", System.Data.DbType.String, action.ExecuteMessage)
                            , _manager.CreateParameter("resultExecuteMessage", System.Data.DbType.String, action.ResultMessage)
                            , _manager.CreateParameter("cancelExecuteMessage", System.Data.DbType.String, action.CancelMessage)
                            , _manager.CreateParameter("resultCancelExecuteMessage", System.Data.DbType.String, action.ResultCancelMessage)
                            );
                }
                //01 60 58 43 56

            }

        }

        public List<Workflow> LoadByExternalId(string key)
        {

            var crc = Crc32.Calculate(key);

            Dictionary<Guid, Workflow> _dic = new Dictionary<Guid, Workflow>();

            var mappingWorkflow = new WorkflowObjectMapping();
            var mappingEvent = new WorkflowObjectMapping();
            var mappintAction = new ActionObjectMapping();

            var workflows = _manager.Read<Workflow>("SELECT [Uuid], [ExternalId], [ExternalIdCrc], [CreationDate], [LastUpdateDate], [WorkflowName], [WorkflowVersion], [Concurency], [Datas] FROM [Workflows] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed = 0]"
                , mappingWorkflow
                , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, crc)
                ).ToList();

            if (workflows.Any())
            {

                foreach (var item in workflows)
                    _dic.Add(item.Uuid, item);

                var events = _manager.Read<EventByKey>("SELECT [Uuid], [WorkflowUuid], [Name], [CreationDate], [EventDate], [FromState], [ToState], [Datas] FROM [WorkflowEvents] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed = 0] ORDER BY [ORDER]"
                    , mappingEvent
                    , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, crc)
                ).ToList();

                Dictionary<Guid, Event> _dic2 = new Dictionary<Guid, Event>();

                foreach (var item in events)
                {
                    _dic[item.WorkflowUuid].Events.Add(item);
                    _dic2.Add(item.Uuid, item);
                }

                var actions = _manager.Read<ActionByKey>("SELECT [Uuid], [EventUuid], [Name], [ExternalIdCrc], [Closed], [ExecuteMessage], [ResultExecuteMessage], [CancelExecuteMessage], [ResultCancelExecuteMessage] FROM [WorkflowActions] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed = 0] ORDER BY [ORDER]"
                    , mappintAction
                    , _manager.CreateParameter("externalIdCrc", System.Data.DbType.UInt32, crc)
                ).ToList();

                foreach (var item in actions)
                    _dic2[item.EventUuid].Actions.Add(item);

            }

            return workflows;
        
        }

        internal Transaction GetTransaction()
        {
            return _manager.GetTransaction();
        }

        private void UpdateWorkflow(Workflow workflow)
        {

            var bytes = Encoding.UTF8.GetBytes(_func(workflow.ExtendedDatas));
            var txt = Convert.ToBase64String(bytes);

            _manager.Update("UPDATE [Workflows] [LastUpdateDate] = CURRENT_TIMESTAMP, [Datas] = @datas WHERE [Uuid] = @uuid"
                , _manager.CreateParameter("uuid", System.Data.DbType.Guid, workflow.Uuid)
                , _manager.CreateParameter("datas", System.Data.DbType.String, txt)
                );

        }


        private readonly object _lock = new object();
        private readonly Func<DynObject, string> _func;
        private readonly SqlManager _manager;

        #region IDisposable Support

        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~ReminderStoreSql() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

}
