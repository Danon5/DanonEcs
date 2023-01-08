using System;
using System.Runtime.CompilerServices;

namespace DanonEcs.Internal.DataStructures
{
    public sealed class SparseSet<T>
    {
        public int Count => m_denseCount;
        
        private T[] m_dense;
        private int[] m_sparse;
        private int[] m_recycledDenseIndices;
        private int m_denseCount;
        private int m_recycledDenseIndexCount;

        public SparseSet()
        {
            m_dense = new T[1];
            m_sparse = new int[1];
            m_recycledDenseIndices = new int[1];
            m_sparse[0] = -1;
        }
        
        public SparseSet(int initialCapacity)
        {
            m_dense = new T[initialCapacity];
            m_sparse = new int[initialCapacity];
            m_recycledDenseIndices = new int[initialCapacity];

            for (var i = 0; i < initialCapacity; i++)
                m_sparse[i] = -1;
        }

        /// <summary>
        /// Adds an element. Automatically resizes the backing arrays if needed.
        /// </summary>
        public ref T Add(int id, in T data)
        {
            // get dense index from either recycled indices or the last occupied dense index + 1 (denseCount)
            var denseIndex = m_recycledDenseIndexCount > 0 ? m_recycledDenseIndices[m_recycledDenseIndexCount--] : m_denseCount;
            
            ResizeSparseIfRequired(id + 1);
            m_sparse[id] = denseIndex;

            ResizeDenseIfRequired(denseIndex + 1);
            m_dense[denseIndex] = data;
            m_denseCount++;
            
            return ref m_dense[denseIndex];
        }

        /// <summary>
        /// Removes an element. Recycles the index for later use.
        /// </summary>
        public void Remove(int id)
        {
            var denseIndex = m_sparse[id];

            // remove from dense and sparse
            m_dense[denseIndex] = default;
            m_sparse[id] = -1;
            m_denseCount--;

            // add to recycled
            ResizeRecycledDenseIndicesIfRequired(m_recycledDenseIndexCount + 1);
            m_recycledDenseIndices[m_recycledDenseIndexCount] = denseIndex;
            m_recycledDenseIndexCount++;
        }

        /// <summary>
        /// Clears the sparse set. Recycles all used indices.
        /// </summary>
        public void Clear()
        {
            ResizeRecycledDenseIndicesIfRequired(m_denseCount);
            
            for (var i = 0; i < m_denseCount; i++)
            {
                m_dense[i] = default;
                m_recycledDenseIndices[i] = i;
            }
            
            for (var i = 0; i < m_sparse.Length; i++)
                m_sparse[i] = -1;

            m_recycledDenseIndexCount = m_denseCount;
            m_denseCount = 0;
        }
        
        /// <summary>
        /// Ensures that the dense array can hold indices up to the required capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResizeDenseIfRequired(int requiredCapacity)
        {
            if (m_dense.Length >= requiredCapacity)
                return;

            var newCapacity = m_dense.Length;
            while (newCapacity < requiredCapacity)
                newCapacity <<= 1;

            Array.Resize(ref m_dense, newCapacity);
        }
        
        /// <summary>
        /// Ensures that the sparse array can hold indices up to the required capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResizeSparseIfRequired(int requiredCapacity)
        {
            if (m_sparse.Length >= requiredCapacity)
                return;

            var oldCapacity = m_sparse.Length;
            var newCapacity = oldCapacity;
            while (newCapacity < requiredCapacity)
                newCapacity <<= 1;

            Array.Resize(ref m_sparse, newCapacity);
            
            for (var i = oldCapacity; i < newCapacity; i++)
                m_sparse[i] = -1;
        }
        
        /// <summary>
        /// Ensures that the recycled dense index array can hold indices up to the required capacity.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResizeRecycledDenseIndicesIfRequired(int requiredCapacity)
        {
            if (m_recycledDenseIndices.Length >= requiredCapacity)
                return;
            
            var newCapacity = m_recycledDenseIndices.Length;
            while (newCapacity < requiredCapacity)
                newCapacity <<= 1;

            Array.Resize(ref m_recycledDenseIndices, newCapacity);
        }
    }
}