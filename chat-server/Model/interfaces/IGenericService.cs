using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models.interfaces
{
	public interface IGenericService<T>
	{
		Task<Result> Get(T model);
		Task<Result> Entry(T model);
		Task<Result> Update(T model);
		Task<Result> Delete(long id);
	}
}
