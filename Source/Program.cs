using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.XPath;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Sourse
{
    public class OrderDB : DbContext
    {
        public OrderDB() : base("OrderDataBase1"){ }
        public DbSet<Order> OrderList { get; set; }
    }
    public class Order
    {
        [Key]
        public string orderNum { get; set; }
        public string orderName { get; set; }
        public string orderClient { get; set; }
        public string ClientPhone { get; set; }
        public string goodName { get; set; }
        public string goodPrice { get; set; }
        public string goodNum { get; set; }
        public int tot { get; set; }
        public Order() { }
        public Order(string num, string name, string client, string clientphone, string goodName,string goodNum, string goodPrice)
        {
            this.orderNum = num;
            this.orderName = name;
            this.orderClient = client;
            this.ClientPhone = clientphone;
            this.goodName = goodName;
            this.goodNum = goodNum;
            this.goodPrice = goodPrice;
            tot = int.Parse(goodNum)*int.Parse(goodPrice);
        }
    }

    public class OrderService
    {
        int count = 0;
        public List<Order> orderList = new List<Order>();
        public void addOrder(Order b)
        {
            orderList.Add(b);
            count++;
            using (var db=new OrderDB())
            {
                db.OrderList.Add(b);
                db.SaveChanges();
            }
        }

        public bool deleteOrder(long i)
        {
            try
            {
                var result = orderList.SingleOrDefault(a => a.orderNum == i.ToString());
                    orderList.Remove(result);
                    count--;
                using (var db = new OrderDB())
                {
                    var order = db.OrderList.SingleOrDefault(o => o.orderNum == i.ToString());
                    db.OrderList.Remove(order);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("该订单不存在");
                return false;
            }

        }

        //查找订单 flag为0按订单号查找，flag为1按订单客人名称 
        public void searchOrder(string s, int flag)
        {
            switch (flag)
            {
                case 0:
                    int t = 0;
                    foreach (Order b in orderList)
                    {
                        if (b.orderNum == s)
                        {
                            t = 1;
                            Console.Write("找到的订单为：");
                            Console.WriteLine(b.orderNum + "  " + b.orderName + "  " + b.orderClient + "  总价：" + b.tot);
                            Console.WriteLine("明细: ");
                        }
                    }
                    if (t == 0) Console.WriteLine("无此订单");
                    break;
                default:
                    int t1 = 0;
                    foreach (Order b in orderList)
                    {
                        if (b.orderClient == s)
                        {
                            t1 = 1;
                            Console.Write("找到的订单为：");
                            Console.WriteLine(b.orderNum + "  " + b.orderName + "  " + b.orderClient + "  总价：" + b.tot);
                            Console.WriteLine("明细: ");
                        }
                        if (t1 == 0) Console.WriteLine("无此订单");
                    }
                    Console.WriteLine("无此订单");
                    break;
            }
        }
        //通过Linq查询订单 flag为0按订单号查找，flag为1按订单客人名称，flag为2查询总价超过10000的订单
        public IEnumerable<Order> searchOrderbyLinq(string s, int flag)
        {
            switch (flag)
            {
                case 0:
                    using (var db = new OrderDB())
                    {
                        return db.OrderList.Where(o => o.orderNum == s);
                    }

                case 1:
                    using (var db = new OrderDB())
                    {
                        return db.OrderList.Where(o => o.orderClient == s);
                    }
                default:
                    using (var db = new OrderDB())
                    {
                        return db.OrderList.Where(o => o.tot>10000);
                    }

            }

        }
        //更改List第num个成员；flag为0，更改订单号，flag为1，更改订单商品名称，flag为2，更改订单客户名称
        public bool changeOrder(int num, string s, int flag)
        {
            try
            {
                switch (flag)
                {
                    case 0:
                        orderList[num-1].orderNum = s;
                        using (var db = new OrderDB())
                        {
                            db.OrderList.Attach(orderList[num-1]);
                            db.Entry(orderList[num - 1]).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                    case 1:
                        orderList[num-1].orderName = s;
                        using (var db = new OrderDB())
                        {
                            db.OrderList.Attach(orderList[num - 1]);
                            db.Entry(orderList[num - 1]).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                    default:
                        orderList[num-1].orderClient = s;
                        using (var db = new OrderDB())
                        {
                            db.OrderList.Attach(orderList[num - 1]);
                            db.Entry(orderList[num - 1]).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        break;
                }
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("需要修改的订单不存在");
                return false;
            }
        }
        public void Export()
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(List<Order>));
            string xmlFileName = @"D:\ComputerWork\Csharp\HomeWork8\M.xml";
            FileStream fs = new FileStream(xmlFileName, FileMode.Create);
            xmlser.Serialize(fs, orderList);
            fs.Close();
        }
        public void toHTML()
        {
            this.Export();
            XmlDocument doc = new XmlDocument();
            doc.Load(@"D:\ComputerWork\Csharp\HomeWork8\M.xml");
            XPathNavigator nav = doc.CreateNavigator();
            XslCompiledTransform xt = new XslCompiledTransform();
            xt.Load(@"D:\ComputerWork\Csharp\HomeWork8\M.xslt");
            FileStream my = new FileStream(@"M.html", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            xt.Transform(nav, null,my);

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}