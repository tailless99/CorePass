using System;
using System.Collections.Generic;

public class EventBusManager : Singleton<EventBusManager> {
    // 이벤트를 담을 딕셔너리
    private Dictionary<Type, List<Action<EventData>>> _eventListeners = new Dictionary<Type, List<Action<EventData>>>();

    /// <summary>
    /// 이벤트를 구독하는 기능
    /// </summary>
    public void Subscribe<T>(Action<T> listener) where T : EventData {
        Type type = typeof(T);
        if (!_eventListeners.ContainsKey(type)) {
            _eventListeners[type] = new List<Action<EventData>>();
        }

        if (listener != null) {
            // T 타입의 이벤트를 받아 EventData 타입으로 변환 후 호출하는 람다 함수 생성
            Action<EventData> wrapper = (e) => listener((T)e);

            _eventListeners[type].Add(wrapper);
        }
    }

    /// <summary>
    /// 이벤트를 실행하는 기능
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
    /// 이벤트 구독을 해지하는 기능
    /// </summary>
    public void Unsubscribe<T>(Action<T> listener) where T : EventData {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type)) {
            _eventListeners[type].Remove(listener as Action<EventData>);
        }
    }
}
