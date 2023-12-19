using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationProgramServices : IApplicationProgramServices
    {
		#region Services

		private IOperationRangeService? OperationRangeService { get; }

		#endregion

		#region Constructor

		public ApplicationProgramServices(IServiceProvider serviceProvider)
		{
			this.OperationRangeService = serviceProvider.GetService<IOperationRangeService>();
        }

        #endregion

        #region Methods

        public async Task<bool?> DeleteOperationRange(Program program, OperationRange operationRange)
        {
            bool result = false;

            if ((program.OperationRangeList != null) 
                    && (this.OperationRangeService != null) 
                    && (await this.OperationRangeService.DeleteOperationRangeAsync(operationRange) == true))
            {
                result = program.OperationRangeList.Remove(operationRange);
            }

            return result;
        }

        public async Task<bool?> DeleteOperationRanges(Program program)
        {
            bool? result = null;

			if (this.OperationRangeService != null)
            {
                 result = await this.OperationRangeService.DeleteOperationsRanges(program.Id);

                if (result == true)
                {
                    program.OperationRangeList?.Clear();
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the operation range to the program
        /// </summary>
        /// <returns><c>true</c>, if operation range was added, <c>false</c> otherwise.</returns>
        public async Task<(bool hasError, bool hasOverLapping)> AddOperationRange(Program program, OperationRange operationRange)
        {
            bool hasError = false;
            bool hasOverLapping = false;
            
            if (program.OperationRangeList?.Count() == 0)
            {
                program.OperationRangeList?.Add(operationRange);
            }
            else
            {
                if (program.OperationRangeList != null)
                {
                    foreach (OperationRange op in program.OperationRangeList)
                    {
                        //Check overlaping
                        if (!(operationRange.StartTime >= op.EndTime) && !(op.StartTime >= operationRange.EndTime) && (op.Day == operationRange.Day))
                        {
                            hasOverLapping = true;
                            break;
                        }
                    }

                    if (hasOverLapping == false)
                    {
                        program.OperationRangeList.Add(operationRange);
                    }
                }
            }

            if (hasOverLapping == false)
            {
                //Http Post OperationRange
                hasError = (this.OperationRangeService == null) || (await this.OperationRangeService.AddOperationRangeAsync(operationRange) != true);
            }            

            return (hasError, hasOverLapping);
        }

        public async Task<ObservableCollection<OperationRange>?> GetOperationsRanges(string programId)
        {
            return (this.OperationRangeService != null) ? await this.OperationRangeService.GetOperationsRanges(programId) : null;
        }

        #endregion
    }
}
