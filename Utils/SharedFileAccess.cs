using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public class SharedFileAccess<T> : IDisposable where T : class
{
    MemoryMappedFile _mmf;
    MemoryMappedViewStream _mmvs;

    int _fileSize = 1024 * 1024 * 2;

    public SharedFileAccess(string mapname)
    {
        _mmf = MemoryMappedFile.CreateOrOpen(mapname, _fileSize, MemoryMappedFileAccess.ReadWrite);
        _mmvs = _mmf.CreateViewStream();
    }

    /// <summary>
    /// 写入映射文件
    /// </summary>
    public void Write(T instance)
    {
        IFormatter formatter = new BinaryFormatter();
        _mmvs.Seek(0, SeekOrigin.Begin);
        formatter.Serialize(_mmvs, instance);
    }
    /// <summary>
    /// 读取
    /// </summary>
    /// <returns></returns>
    public T Read()
    {
        IFormatter formatter = new BinaryFormatter();
        formatter.Binder = new UBinder();
        _mmvs.Seek(0, SeekOrigin.Begin);
        T obj = formatter.Deserialize(_mmvs) as T;

        return obj;
    }


    public void Dispose()
    {
        _mmvs?.Close();
        _mmf?.Dispose();
    }
}

public class UBinder : SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        return assembly.GetType(typeName);
    }
}



public struct SmallExchangeUnit
{
    public int Instruction;
}



public class SharedFileAccess
{
    string _mapname;
    bool _canelFlag;
    Thread _thread;

    public SharedFileAccess(string mapname)
    {
        _mapname = mapname;
    }


    public delegate void MessageReceivedHandler(object result);
    public MessageReceivedHandler MessageReceived;

    public SmallExchangeUnit OutgoingUnit;


    public void StartListen()
    {
        if (_thread != null)
        {
            return;
        }

        Type exhangeUnitType = typeof(SmallExchangeUnit);
        SmallExchangeUnit defaultUnit = default(SmallExchangeUnit);
        int dataSize = Marshal.SizeOf(exhangeUnitType);


        _thread = new Thread(() =>
        {
            // 不dispose即保留
            using (var reader = MemoryMappedFile.CreateOrOpen(_mapname, dataSize))
            using (var writer = MemoryMappedFile.CreateOrOpen(_mapname, dataSize))
            {
                while (_canelFlag == false)
                {
                    SmallExchangeUnit incomingMessage;

                    using (var readAccessor = reader.CreateViewAccessor(0, dataSize, MemoryMappedFileAccess.Read))
                    using (var writeAccessor = writer.CreateViewAccessor(0, dataSize, MemoryMappedFileAccess.Write))
                    {
                        try
                        {
                            readAccessor.Read(0, out incomingMessage);

                            // 处理的收到消息
                            if (incomingMessage.Instruction != 0)
                            {
                                if (MessageReceived != null)
                                {
                                    IAsyncResult outcome = MessageReceived.BeginInvoke(incomingMessage, null, null);
                                    outcome.AsyncWaitHandle.WaitOne();
                                    MessageReceived.EndInvoke(outcome);

                                    writeAccessor.Write(0, ref defaultUnit);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.ToString());
                        }


                        if (OutgoingUnit.Instruction != 0)
                        {
                            try
                            {
                                writeAccessor.Write(0, ref OutgoingUnit);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    Thread.Sleep(1000);

                }
            }



        });
        _thread.Priority = ThreadPriority.BelowNormal;
        _thread.Start();
    }

    public void Stop()
    {
        _canelFlag = true;
        //_thread.Join(500);
    }

    public void SendOnce(SmallExchangeUnit ongoingUnit)
    {
        int dataSize = Marshal.SizeOf(typeof(SmallExchangeUnit));


        using (var writer = MemoryMappedFile.CreateOrOpen(_mapname, dataSize))
        using (var writeAccessor = writer.CreateViewAccessor(0, dataSize, MemoryMappedFileAccess.Write))
        {
            writeAccessor.Write(0, ref ongoingUnit);
        }

    }

}


