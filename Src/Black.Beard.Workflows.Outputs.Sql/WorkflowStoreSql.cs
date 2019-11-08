using Bb.ComponentModel;
using Bb.ComponentModel.Accessors;
using Bb.Dao;
using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bb.Workflows.Outputs.Sql
{

    public class WorkflowStoreSql : IDisposable
    {


        public WorkflowStoreSql(SqlManager manager, WorkflowFactory factory)
        {
            this._factory = factory;
            this._manager = manager;

        }

        public void Save(Workflow workflow)
        {


            if (workflow.Change == ChangeEnum.New)
            {
                _manager.Update("INSERT INTO [Workflows] ([Uuid], [ExternalId], [ExternalIdCrc], [CreationDate], [LastUpdateDate], [WorkflowName], [WorkflowVersion], [Closed], [Concurency]) VALUES (@Uuid, @externalId, @externalIdCrc, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @workflowName, @workflowVersion, 0, @concurency)"
                , _manager.CreateParameter("Uuid", System.Data.DbType.Guid, workflow.Uuid)
                , _manager.CreateParameter("externalId", System.Data.DbType.String, workflow.ExternalId)
                , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, Crc32.Calculate(workflow.ExternalId))
                , _manager.CreateParameter("workflowName", System.Data.DbType.String, workflow.WorkflowName)
                , _manager.CreateParameter("workflowVersion", System.Data.DbType.Int16, workflow.Version)
                , _manager.CreateParameter("concurency", System.Data.DbType.Int32, workflow.Concurency)
                );

                workflow.Change = ChangeEnum.None;

            }

            int index = 0;
            foreach (var @event in workflow.Events)
            {

                index++;

                if (@event.Change == ChangeEnum.New)
                {

                    string txtStep = string.Empty;
                    string txtWorkflow = string.Empty;
                    if (workflow.ExtendedDatas() != null)
                    {
                        txtWorkflow = _factory.Serializer.Serialize(workflow);
                        txtStep = _factory.Serializer.Serialize(@event);
                    }

                    _manager.Update("INSERT INTO [WorkflowEvents] ([Uuid], [WorkflowUuid], [Name], [order], [ExternalIdCrc], [Closed], [CreationDate], [EventDate], [FromState], [ToState], [ToStateCrc], [Datas], [DatasWorkflow]) VALUES (@uuid, @workflowUuid, @name, @order, @externalIdCrc, 0, CURRENT_TIMESTAMP, @eventDate, @fromState, @toState, @toStateCrc, @datas, @datasWorkflow)"
                        , _manager.CreateParameter("Uuid", System.Data.DbType.Guid, @event.Uuid)
                        , _manager.CreateParameter("workflowUuid", System.Data.DbType.Guid, workflow.Uuid)
                        , _manager.CreateParameter("name", System.Data.DbType.String, @event.Name)
                        , _manager.CreateParameter("order", System.Data.DbType.Int32, index)
                        , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, Crc32.Calculate(workflow.ExternalId))
                        , _manager.CreateParameter("eventDate", System.Data.DbType.DateTimeOffset, @event.EventDate)
                        , _manager.CreateParameter("fromState", System.Data.DbType.String, @event.FromState)
                        , _manager.CreateParameter("toState", System.Data.DbType.String, @event.ToState)
                        , _manager.CreateParameter("toStateCrc", System.Data.DbType.Int64, Crc32.Calculate(@event.ToState))
                        , _manager.CreateParameter("datas", System.Data.DbType.String, txtStep)
                        , _manager.CreateParameter("datasWorkflow", System.Data.DbType.String, txtStep)
                        );
                    @event.Change = ChangeEnum.None;
                }

                int index2 = 0;

                foreach (var action in @event.Actions)
                {

                    index2++;

                    if (action.Change == ChangeEnum.New)
                        _manager.Update("INSERT INTO [WorkflowActions] ([Uuid], [EventUuid], [Name], [order], [ExternalIdCrc], [Closed], [CreationDate], [LasteUpdateDate], [ExecuteMessage]) VALUES (@uuid, @eventUuid, @name, @order, @externalIdCrc, 0, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @executeMessage)"
                            , _manager.CreateParameter("uuid", System.Data.DbType.Guid, action.Uuid)
                            , _manager.CreateParameter("EventUuid", System.Data.DbType.Guid, @event.Uuid)
                            , _manager.CreateParameter("name", System.Data.DbType.String, action.Name)
                            , _manager.CreateParameter("order", System.Data.DbType.Int32, index2)
                            , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, Crc32.Calculate(workflow.ExternalId))
                            , _manager.CreateParameter("executeMessage", System.Data.DbType.String, _factory.Serializer.Serialize(action.ExecuteMessage))
                            );


                    else if (action.Change == ChangeEnum.Updated)
                        _manager.Update("UPDATE [WorkflowActions] SET [LasteUpdateDate] = CURRENT_TIMESTAMP, [ExecuteMessage] = @executeMessage, [ResultExecuteMessage] = @resultExecuteMessage, [CancelExecuteMessage] = @cancelExecuteMessage, [ResultCancelExecuteMessage] =@resultCancelExecuteMessage WHERE [Uuid] = @uuid"
                            , _manager.CreateParameter("uuid", System.Data.DbType.Guid, action.Uuid)
                            , _manager.CreateParameter("executeMessage", System.Data.DbType.String, _factory.Serializer.Serialize(action.ExecuteMessage))
                            , _manager.CreateParameter("resultExecuteMessage", System.Data.DbType.String, _factory.Serializer.Serialize(action.ResultMessage))
                            , _manager.CreateParameter("cancelExecuteMessage", System.Data.DbType.String, _factory.Serializer.Serialize(action.CancelMessage))
                            , _manager.CreateParameter("resultCancelExecuteMessage", System.Data.DbType.String, _factory.Serializer.Serialize(action.ResultCancelMessage))
                            );

                    action.Change = ChangeEnum.None;

                }

            }

        }

        public List<Workflow> LoadByExternalId(string key)
        {

            var crc = Crc32.Calculate(key);

            Dictionary<Guid, Workflow> _dic = new Dictionary<Guid, Workflow>();

            var mappingWorkflow = new WorkflowObjectMapping(_factory);
            var mappingEvent = new EventObjectMapping(_factory);
            var mappintAction = new ActionObjectMapping();

            var workflows = _manager.Read<Workflow>("SELECT [Uuid], [ExternalId], [CreationDate], [LastUpdateDate], [WorkflowName], [WorkflowVersion], [Concurency] FROM [Workflows] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed] = 0"
                , mappingWorkflow
                , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, crc)
                )
                .ToList();

            if (workflows.Any())
            {

                foreach (var item in workflows)
                    _dic.Add(item.Uuid, item);

                var events = _manager.Read<EventByKey>("SELECT [Uuid], [WorkflowUuid], [Name], [CreationDate], [EventDate], [FromState], [ToState], [Datas], [DatasWorkflow] FROM [WorkflowEvents] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed] = 0 ORDER BY [ORDER]"
                    , mappingEvent
                    , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, crc)
                ).ToList();

                Dictionary<Guid, Event> _dic2 = new Dictionary<Guid, Event>();

                foreach (var item in events)
                    if (_dic.TryGetValue(item.WorkflowUuid, out Workflow w)) // prevent unfortunaly crc collision
                    {
                        w.Events.Add(item);
                        _dic2.Add(item.Uuid, item);
                        _factory.Serializer.Populate(w, item.Tag);
                    }

                var actions = _manager.Read<ActionByKey>("SELECT [Uuid], [EventUuid], [Name], [ExternalIdCrc], [Closed], [ExecuteMessage], [ResultExecuteMessage], [CancelExecuteMessage], [ResultCancelExecuteMessage] FROM [WorkflowActions] WHERE [ExternalIdCrc] = @externalIdCrc AND [Closed] = 0 ORDER BY [ORDER]"
                    , mappintAction
                    , _manager.CreateParameter("externalIdCrc", System.Data.DbType.Int64, crc)
                ).ToList();

                foreach (var item in actions)
                    if (_dic2.TryGetValue(item.EventUuid, out Event e)) // prevent unfortunaly crc collision
                        e.Actions.Add(item);

            }

            return workflows;

        }

        internal Transaction GetTransaction()
        {
            return _manager.GetTransaction();
        }


        private readonly object _lock = new object();
        private readonly WorkflowFactory _factory;
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
