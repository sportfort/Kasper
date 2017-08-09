using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kasper
{
    public class ThreadSafeQueue<T>
    {
        //основной контейнер
        private readonly Queue<T> _items;
        //очередь запросов на случай пустого контейнера
        private readonly Queue<ItemEnqueuedHandler> _delayedPops;
        private delegate void ItemEnqueuedHandler(T item);

        public ThreadSafeQueue()
        {
            _items = new Queue<T>();
            _delayedPops = new Queue<ItemEnqueuedHandler>();
        }

        /// <summary>
        /// Возвращает элемент из очереди. Если очередь пустая, то ждет нового элемента.
        /// При нескольких попытках извлечения элементов из пустой очереди, сами запросы становятся в очередь.
        /// </summary>
        /// <returns></returns>
        public async Task<T> PopAsync()
        {
            Task<T> result;
            //лочим, потому что было требования работы в разных потоках
            lock (_items)
            {
                if (_items.Count > 0)
                {
                    result = Task.FromResult(_items.Dequeue());
                }
                else
                {
                    //создаем задачу для возврата вызывающему методу
                    //важно: до этого задания я ни разу не сталкивался с TaskCompletionSource, нагуглил в процессе решения, без гугла не справился бы
                    var itemEnqueuedTaskSource = new TaskCompletionSource<T>(); 
                    result = itemEnqueuedTaskSource.Task;
                    
                    //связываем задачу и событие добавления нового элемента в очередь
                    _delayedPops.Enqueue(item => itemEnqueuedTaskSource.SetResult(item));
                }
            }
            return await result;
        }

        public void Push(T item)
        {
            //лочим, потому что было требования работы в разных потоках
            lock (_items)
            {
                //если никто не ждет элемента, то просто добавляем его в контейнер
                if (_delayedPops.Count == 0)
                {
                    _items.Enqueue(item);
                }
                //если новый элемент уже ждут, то сразу передаем его без добавления в контейнер
                else
                {
                    var delayedPop = _delayedPops.Dequeue();
                    delayedPop.Invoke(item);
                }
            }
        }
    }
}
