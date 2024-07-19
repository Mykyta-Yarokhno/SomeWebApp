using System.Collections.Concurrent;
using WebApplication1.Models;

namespace WebApplication1
{
   
    public class CoffeeService
    {
        public class CoffeeMachine
        {
            private readonly CoffeeService _coffeeService;
            private readonly string _name;

            private bool _isTurnedOn = false;

            public bool isTurnedOn => _isTurnedOn;

            public CoffeeMachineInfo Info => new CoffeeMachineInfo { State = _isTurnedOn ? CoffeeMachineState.On : CoffeeMachineState.Off, Name = _name };

            public CoffeeMachine(string name, CoffeeService coffeeService)
            {
                _name= name;
                _coffeeService = coffeeService;          
            }

            private void MachineRunning()
            {
                DateTime lastProcessedOrder =DateTime.Now;
                do
                {
                    do
                    {
                        if (_coffeeService._awaitingOrders.TryDequeue(out var order))
                        {
                            CoffeePreparingProcess(order);
                            lastProcessedOrder = DateTime.Now;
                        }
                        else
                        {
                            if ((DateTime.Now - lastProcessedOrder).Minutes > 1)
                            {
                                TurnOff();
                            }
                        }
                    } while (_isTurnedOn);

                    if (_coffeeService._awaitingOrders.Count() > 0)
                    {
                        _isTurnedOn = true;


                    }
                } while (_isTurnedOn);
            }

            public void TurnOn()
            {
                if (_isTurnedOn) return;

                lock (this)
                {
                    if (_isTurnedOn) return;
                    
                    

                    _isTurnedOn = true;
                }

                Task.Run(MachineRunning);
         
            }

            public void TurnOff()
            {
                _isTurnedOn = false;
            }

            public void CoffeePreparingProcess(CoffeeOrderInfo orderInfo)
            {
                orderInfo.Status = OrderStatus.Preparing;
                Thread.Sleep(20000);
                orderInfo.Status = OrderStatus.Ready;                
            }
            
        }

        volatile private int _orderId;
        private ConcurrentQueue<CoffeeOrderInfo> _awaitingOrders = new ();
        private ConcurrentDictionary<int, CoffeeOrderInfo> _registeredOrders = new ();
       
        private List<CoffeeMachine> _coffeeMachines = new ();

        public CoffeeMachine? GetCoffeeMachine()
        {
            return _coffeeMachines.FirstOrDefault();
        }

        public CoffeeService()
        {
            _coffeeMachines.Add(new CoffeeMachine("B2",this));
        }

        public CoffeeOrderInfo DoMakeCoffee(CoffeeSettings? coffeeSettings)
        {
            if(!_coffeeMachines.FirstOrDefault().isTurnedOn)
            {
                throw new Exception("Coffee machine is turned off. You can't make coffee");
            }

            var order = 
                new CoffeeOrderInfo 
                { 
                    CoffeeSettings = coffeeSettings,
                    OrderDate = DateTime.Now, 
                    Status = OrderStatus.Created, 
                    OrderID = Interlocked.Increment(ref _orderId) 
                };
            _registeredOrders.TryAdd(order.OrderID, order );
            order.Status = OrderStatus.Awaiting;
            _awaitingOrders.Enqueue(order);

            return order;
            
        }

        public CoffeeOrderInfo? LookUpOrder(int orderId)
        {
            _registeredOrders.TryGetValue(orderId, out var retVal);
            
             return retVal;
            
           
            
        }

        public IReadOnlyCollection<CoffeeOrderInfo> GetOrders(OrderStatus? displayOrders)
        {
            if(displayOrders.HasValue)
            {
                return _registeredOrders.Values.Where((x) => x.Status == displayOrders).ToList();
            }

            return _registeredOrders.Values.ToList();
        }

           



    }

}
