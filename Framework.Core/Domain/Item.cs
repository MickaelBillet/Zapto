using Framework.Core.Base;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Core.Domain
{
	public class Item : ObservableObject
	{
		private string id = string.Empty;
		private DateTime date;

		#region Property

		[JsonIgnore] 
		protected bool Disposed { get; set; } = false; // to detect redundant calls

		public string Id
		{
			get { return id; }

			set { SetProperty<string>(ref id, value); }
		}

		public DateTime Date
		{
			get { return date; }

			set { SetProperty<DateTime>(ref date, value); }
		}

		#endregion

		#region Constructor

		public Item()
		{
			this.Date = Clock.Now;
		}

		#endregion

		#region Method

		public virtual T Clone<T>() where T : class 
		{
			if (this is null)
			{
				return default;
			}

			string json = JsonSerializer.Serialize<T>(this as T);

			// In the PCL we do not have the BinaryFormatter
			return JsonSerializer.Deserialize<T>(json);
		}

		#endregion
	}
}
