using System.Text.Json.Serialization;

namespace Common.EventBus
{
    /// <summary>
    /// Событие интеграции.
    /// </summary>
    public class IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Идентификатор события интеграции.
        /// </summary>
        [JsonInclude]
        public Guid Id { get; private init; }

        /// <summary>
        /// Дата создания.
        /// </summary>
        [JsonInclude]
        public DateTime CreationDate { get; private init; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор события интеграции.</param>
        /// <param name="createDate">Дата создания.</param>
        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        #endregion Constrcutors
    }
}
