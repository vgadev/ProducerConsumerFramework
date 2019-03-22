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
    public interface IProcessingOutput<TType>
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

    public interface IProcessingStage : IWaitable
    {
        /// <summary>
        /// Starts the main execution loop of the stage.
        /// </summary>
        void Start();

        /// <summary>
        /// The number of parallel threads that this stage will use for processing.
        /// </summary>
        int Parallelism { get; }

        /// <summary>
        /// The name of the pipeline stage.
        /// </summary>
        string Name { get; }
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
        void SetInput(IEnumerable<TInputType> inputEnumerator);
    }

    public interface IProcessingStage<TInputType, TOutputType> : IProcessor<TInputType, TOutputType>, IBatchProcessor<TInputType>, IProcessingOutput<TOutputType>
    {
    }

    public interface IProcessingStage<TInputType> : IProcessor<TInputType>, IBatchProcessor<TInputType>
    {
    }
}
