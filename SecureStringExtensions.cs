using System;
using System.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace  CardCells.utility
{

	//Properties.Settings.Default.Password = Convert.ToBase64String(securePassword.GetProtectedData());
		//- See more at: http://www.griffinscs.com/blog/?p=12#sthash.j3IGZb4p.dpuf
//	string encPassword = Properties.Settings.Default.Password;
//if (!string.IsNullOrEmpty(encPassword))
//{
//SecureString passwordString = new SecureString();
//passwordString.AppendProtectedData(Convert.FromBase64String(encPassword));
//}
	public static class SecureStringExtensions
	{
	
		public static unsafe SecureString Secure(this string source)
		{
        if (source == null)
            return null;
        if (source.Length == 0)
            return new SecureString();

        fixed (char* pChars = source.ToCharArray())
        {
            SecureString secured = new SecureString(pChars, source.Length);
            return secured;
        }
		}

		public static string Unsecure(this SecureString source)
		{
			if (source == null)
				return null;

			IntPtr bstr = Marshal.SecureStringToBSTR(source);
			try
			{
				return Marshal.PtrToStringUni(bstr);
			}
			finally
			{
				Marshal.ZeroFreeBSTR(bstr);
			}
		}
		public static byte[] GetProtectedData(this SecureString self)
		{
			byte[] out_buffer;
			DATA_BLOB out_blob = new DATA_BLOB();
			IntPtr unmanagedString = Marshal.SecureStringToBSTR(self);
			try
			{
				DATA_BLOB in_blob = new DATA_BLOB();
				in_blob.cbData = Marshal.ReadInt32(unmanagedString, -4); //read unicode size from BSTR
				in_blob.pbData = unmanagedString;
				if (!CryptProtectData(ref in_blob, string.Empty, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
					CryptProtectFlags.CRYPTPROTECT_UI_FORBIDDEN, ref out_blob))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				out_buffer = new byte[out_blob.cbData];
				Marshal.Copy(out_blob.pbData, out_buffer, 0, out_buffer.Length);
			}
			finally
			{
				//free buffers...
				if (out_blob.pbData != IntPtr.Zero)
				{
					ZeroMemory(out_blob.pbData, out_blob.cbData);
					LocalFree(out_blob.pbData);
				}
				Marshal.ZeroFreeBSTR(unmanagedString); //...especially this one
			}
			return out_buffer;
		}

		public static void AppendProtectedData(this SecureString self, byte[] protectedData)
		{

			if (protectedData.Length == 0) return;
			GCHandle unmanagedProtectedData = new GCHandle();
			DATA_BLOB out_blob = new DATA_BLOB();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				unmanagedProtectedData = GCHandle.Alloc(protectedData, GCHandleType.Pinned);
				DATA_BLOB in_blob = new DATA_BLOB();
				in_blob.cbData = protectedData.Length;
				in_blob.pbData = unmanagedProtectedData.AddrOfPinnedObject();
				DATA_BLOB cryptoapi_blob3 = new DATA_BLOB();
				if (!CryptUnprotectData(ref in_blob, string.Empty, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero,
					CryptProtectFlags.CRYPTPROTECT_UI_FORBIDDEN, ref out_blob))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				for (int i = 0; i < out_blob.cbData; i += 2)
				{
					self.AppendChar((char)Marshal.ReadInt16(out_blob.pbData, i));
				}

			}
			finally
			{
				if (unmanagedProtectedData != null)
				{
					unmanagedProtectedData.Free();
				}
				if (out_blob.pbData != IntPtr.Zero)
				{
					ZeroMemory(out_blob.pbData, out_blob.cbData);
					LocalFree(out_blob.pbData);
				}
			}
		}

		[DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptProtectData(
			ref DATA_BLOB pDataIn,
			String szDataDescr,
			IntPtr pOptionalEntropy,
			IntPtr pvReserved,
			IntPtr pPromptStruct,
			CryptProtectFlags dwFlags,
			ref DATA_BLOB pDataOut
		);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct DATA_BLOB
		{
			public int cbData;
			public IntPtr pbData;
		}

		[Flags]
		private enum CryptProtectFlags
		{
			// for remote-access situations where ui is not an option
			// if UI was specified on protect or unprotect operation, the call
			// will fail and GetLastError() will indicate ERROR_PASSWORD_RESTRICTION
			CRYPTPROTECT_UI_FORBIDDEN = 0x1,

			// per machine protected data -- any user on machine where CryptProtectData
			// took place may CryptUnprotectData
			CRYPTPROTECT_LOCAL_MACHINE = 0x4,

			// force credential synchronize during CryptProtectData()
			// Synchronize is only operation that occurs during this operation
			CRYPTPROTECT_CRED_SYNC = 0x8,

			// Generate an Audit on protect and unprotect operations
			CRYPTPROTECT_AUDIT = 0x10,

			// Protect data with a non-recoverable key
			CRYPTPROTECT_NO_RECOVERY = 0x20,


			// Verify the protection of a protected blob
			CRYPTPROTECT_VERIFY_PROTECTION = 0x40
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void ZeroMemory(IntPtr handle, int length);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		[DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CryptUnprotectData(
			ref DATA_BLOB pDataIn,
			String szDataDescr,
			IntPtr pOptionalEntropy,
			IntPtr pvReserved,
			IntPtr pPromptStruct,
			CryptProtectFlags dwFlags,
			ref DATA_BLOB pDataOut
		);
	}
}