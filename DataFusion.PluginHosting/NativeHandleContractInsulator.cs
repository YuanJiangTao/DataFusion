using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.AddIn.Contract;

namespace DataFusion.PluginHosting
{
    public class NativeHandleContractInsulator : MarshalByRefObject, INativeHandleContract
    {
        private readonly INativeHandleContract _source;

        public NativeHandleContractInsulator(INativeHandleContract source)
        {
            _source = source;
        }

        public int AcquireLifetimeToken()
        {
            return _source.AcquireLifetimeToken();
        }

        public IntPtr GetHandle()
        {
            return _source.GetHandle();
        }

        public int GetRemoteHashCode()
        {
            return _source.GetRemoteHashCode();
        }

        public IContract QueryContract(string contractIdentifier)
        {
            return _source.QueryContract(contractIdentifier);
        }

        public bool RemoteEquals(IContract contract)
        {
            return _source.RemoteEquals(contract);
        }

        public string RemoteToString()
        {
            return _source.RemoteToString();
        }

        public void RevokeLifetimeToken(int token)
        {
            _source.RevokeLifetimeToken(token);
        }
        public override object InitializeLifetimeService()
        {
            return null; // live forever
        }
    }
}
