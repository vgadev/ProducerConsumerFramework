using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VGA.Tools.ProducerConsumer
{
    /// <summary>
    /// Represents a type that can provide an output, 
    /// that is usable by a pipeline stage.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public interface IOutputProvider<TType>
    {
        /// <summary>
        /// Gets an iterator to the output of a pipeline stage.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TType> GetOutput();
    }


    public interface IWaitable
    {
        /// <summary>
        /// Blocks the calling thread until the execution of the stage is completed.
        /// </summary>
        void WaitForCompletion();
    }

    /// <summary>
    /// Represents a pipeline stage.
    /// </summary>
    /// <typeparam name="TInputType"></typeparam>
    /// <typeparam name="TOutputType"></typeparam>
    public interface IProcessor<TInputType>
    {
        void ProcessSingleItem(TInputType inputItem);
    }

    public interface IProcessor<TInputType, TOutputType>
    {
        TOutputType ProcessSingleItem(TInputType inputItem);
    }

    public interface IBatchProcessor<TInputType>
    {
        bool IsParallel { get; }
        int ThreadCount { get; }

        string Name { get; }
        void SetInput(IEnumerable<TInputType> inputEnumerator);
    }

    public interface IProcessingStage<TInputType, TOutputType> : IProcessor<TInputType, TOutputType>, IBatchProcessor<TInputType>, IOutputProvider<TOutputType>, IWaitable
    {
    }

    public interface IProcessingStage<TInputType> : IProcessor<TInputType>, IBatchProcessor<TInputType>, IWaitable
    {
    }
}
