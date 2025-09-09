using System;
using System.Collections.Generic;

public class EventBusManager : Singleton<EventBusManager> {
    // �̺�Ʈ�� ���� ��ųʸ�
    private Dictionary<Type, List<Action<EventData>>> _eventListeners = new Dictionary<Type, List<Action<EventData>>>();

    /// <summary>
    /// �̺�Ʈ�� �����ϴ� ���
    /// </summary>
    public void Subscribe<T>(Action<T> listener) where T : EventData {
        Type type = typeof(T);
        if (!_eventListeners.ContainsKey(type)) {
            _eventListeners[type] = new List<Action<EventData>>();
        }

        if (listener != null) {
            // T Ÿ���� �̺�Ʈ�� �޾� EventData Ÿ������ ��ȯ �� ȣ���ϴ� ���� �Լ� ����
            Action<EventData> wrapper = (e) => listener((T)e);

            _eventListeners[type].Add(wrapper);
        }
    }

    /// <summary>
    /// �̺�Ʈ�� �����ϴ� ���
    /// </summary>
    public void Publish<T>(T eventData) where T : EventData {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type)) {
            foreach (var listener in _eventListeners[type]) {
                listener?.Invoke(eventData);
            }
        }
    }

    /// <summary>
    /// �̺�Ʈ ������ �����ϴ� ���
    /// </summary>
    public void Unsubscribe<T>(Action<T> listener) where T : EventData {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type)) {
            _eventListeners[type].Remove(listener as Action<EventData>);
        }
    }
}
