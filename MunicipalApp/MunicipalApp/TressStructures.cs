using System;
using System.Collections.Generic;

namespace MunicipalApp
{
    public class TreeNode<T>
    {
        public T Value;
        public TreeNode<T> Left, Right;
        public TreeNode(T value) { Value = value; }
    }

    public class BinarySearchTree<T> where T : IComparable<T>
    {
        public TreeNode<T> Root;

        public void Insert(T value)
        {
            Root = Insert(Root, value);
        }

        public TreeNode<T> Insert(TreeNode<T> node, T value)
        {
            if (node == null) return new TreeNode<T>(value);
            int cmp = value.CompareTo(node.Value);
            if (cmp < 0) node.Left = Insert(node.Left, value);
            else if (cmp > 0) node.Right = Insert(node.Right, value);
            return node;
        }

        public bool Search(T value)
        {
            var node = Root;
            while (node != null)
            {
                int cmp = value.CompareTo(node.Value);
                if (cmp == 0) return true;
                node = (cmp < 0) ? node.Left : node.Right;
            }
            return false;
        }

        public List<T> InOrderTraversal()
        {
            var result = new List<T>();
            InOrder(Root, result);
            return result;
        }

        public void InOrder(TreeNode<T> node, List<T> list)
        {
            if (node == null) return;
            InOrder(node.Left, list);
            list.Add(node.Value);
            InOrder(node.Right, list);
        }
    }

    // Simplified AVL (balances using rotations)
    public class AVLNode<T> where T : IComparable<T>
    {
        public T Value;
        public AVLNode<T> Left, Right;
        public int Height;
        public AVLNode(T value) { Value = value; Height = 1; }
    }

    public class AVLTree<T> where T : IComparable<T>
    {
        public AVLNode<T> Root;

        private int Height(AVLNode<T> n) => n?.Height ?? 0;
        private int Balance(AVLNode<T> n) => n == null ? 0 : Height(n.Left) - Height(n.Right);

        public void Insert(T value) => Root = Insert(Root, value);

        public AVLNode<T> Insert(AVLNode<T> node, T value)
        {
            if (node == null) return new AVLNode<T>(value);
            if (value.CompareTo(node.Value) < 0) node.Left = Insert(node.Left, value);
            else node.Right = Insert(node.Right, value);
            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
            int balance = Balance(node);
            if (balance > 1 && value.CompareTo(node.Left.Value) < 0) return RotateRight(node);
            if (balance < -1 && value.CompareTo(node.Right.Value) > 0) return RotateLeft(node);
            if (balance > 1 && value.CompareTo(node.Left.Value) > 0) { node.Left = RotateLeft(node.Left); return RotateRight(node); }
            if (balance < -1 && value.CompareTo(node.Right.Value) < 0) { node.Right = RotateRight(node.Right); return RotateLeft(node); }
            return node;
        }

        public AVLNode<T> RotateRight(AVLNode<T> y)
        {
            var x = y.Left;
            var T2 = x.Right;
            x.Right = y;
            y.Left = T2;
            y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));
            x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));
            return x;
        }

        public AVLNode<T> RotateLeft(AVLNode<T> x)
        {
            var y = x.Right;
            var T2 = y.Left;
            y.Left = x;
            x.Right = T2;
            x.Height = 1 + Math.Max(Height(x.Left), Height(x.Right));
            y.Height = 1 + Math.Max(Height(y.Left), Height(y.Right));
            return y;
        }

        public List<T> InOrderTraversal()
        {
            var result = new List<T>();
            Traverse(Root, result);
            return result;
        }

        public void Traverse(AVLNode<T> node, List<T> list)
        {
            if (node == null) return;
            Traverse(node.Left, list);
            list.Add(node.Value);
            Traverse(node.Right, list);
        }
    }
}
