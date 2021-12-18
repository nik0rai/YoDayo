using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Program
    {
        public enum TreeTraversalType
        {
            PreOrder = 1,
            InOrder = 2,
            PostOrder = 3,
            LevelOrder = 4
        }
        public class Selector<T>
        {
            TreeTraversalType choise;
            MyGraph<T> Tree;
            public Selector(TreeTraversalType TraversalType, MyGraph<T> graph)
            {
                choise = TraversalType;
                Tree = graph;
            }
            public IEnumerator<Node<T>> GetEnumerator()
            {
                switch (choise)
                {
                    case TreeTraversalType.PreOrder:
                        return new PreOrderEnumerator<T>(Tree);
                    case TreeTraversalType.InOrder:
                        return new InOrderEnumerator<T>(Tree);
                    case TreeTraversalType.PostOrder:
                        return new PostOrderEnumerator<T>(Tree);
                    default:
                        return new LevelOrderEnumerator<T>(Tree);
                }
            }
        }
        public class LevelOrderEnumerator<T> : IEnumerator<Node<T>>
        {
            private MyGraph<T> tree;
            int pos;
            public Node<T> node;
            private Node<T> temp;

            Queue<Node<T>> q = new();
            Node<T> IEnumerator<Node<T>>.Current
            {
                get
                {
                    return node;
                }
            }
            object IEnumerator.Current => throw new NotImplementedException();
            public LevelOrderEnumerator(MyGraph<T> tree)
            {
                this.tree = tree;
                pos = -1;
                node = default;
                temp = tree.Root;
                q.Enqueue(temp);
            }
            public void Dispose()
            {

            }
            public bool MoveNext()
            {
                while (q.Count > 0)
                {
                    temp = q.Peek();
                    if (temp != null)
                    {
                        node = temp;
                        q.Dequeue();
                        if (node.Left != null)
                            q.Enqueue(node.Left);

                        if (node.Right != null)
                            q.Enqueue(node.Right);
                        return true;
                    }
                }
                return false;
            }
            public void Reset() => pos = -1;
        }
        public class PreOrderEnumerator<T> : IEnumerator<Node<T>>
        {
            private MyGraph<T> tree;
            int pos;
            public Node<T> node;
            private Node<T> temp;

            Stack<Node<T>> stack = new();
            Node<T> IEnumerator<Node<T>>.Current
            {
                get
                {
                    return node;
                }
            }
            object IEnumerator.Current => throw new NotImplementedException();
            public PreOrderEnumerator(MyGraph<T> tree)
            {
                this.tree = tree;
                pos = -1;
                node = default;
                temp = tree.Root;
                stack.Push(temp);
            }
            public void Dispose()
            {
            }
            public bool MoveNext()
            {
                while (stack.Count > 0)
                {
                    temp = stack.Pop();
                    node = temp;

                    if (temp.Right != null)
                        stack.Push(temp.Right);   
                    
                    if (temp.Left != null)                   
                        stack.Push(temp.Left);
                    
                    return true;
                }
                return false;
            }

            public void Reset() => pos = -1;
        }
        public class InOrderEnumerator<T> : IEnumerator<Node<T>>
        {
            private MyGraph<T> tree;
            int pos;
            public Node<T> node;
            private Node<T> temp;

            Stack<Node<T>> stack = new();

            Node<T> IEnumerator<Node<T>>.Current
            {
                get
                {
                    return node;
                }
            }
            object IEnumerator.Current => throw new NotImplementedException();
            public InOrderEnumerator(MyGraph<T> tree)
            {
                this.tree = tree;
                pos = -1;
                node = default;
                temp = tree.Root;
            }
            public void Dispose()
            {
            }
            public bool MoveNext()
            {
                while (stack.Count > 0 || temp != null)
                {
                    if (temp != null)
                    {
                        stack.Push(temp);
                        temp = temp.Left;
                    }
                    else
                    {
                        temp = stack.Pop();
                        node = temp;
                        temp = temp.Right;
                        return true;
                    }
                }
                return false;
            }
            public void Reset() => pos = -1;
        }

        public class PostOrderEnumerator<T> : IEnumerator<Node<T>>
        {
            private MyGraph<T> tree;
            int pos;
            public Node<T> node;
            private Node<T> temp, chekerNode, prev;

            Stack<Node<T>> stack = new();
            Node<T> IEnumerator<Node<T>>.Current
            {
                get
                {
                    return node;
                }
            }
            object IEnumerator.Current => throw new NotImplementedException();
            public Node<T> Current => throw new NotImplementedException();
            public PostOrderEnumerator(MyGraph<T> tree)
            {
                this.tree = tree;
                pos = -1;
                node = default;
                prev = default;
                chekerNode = default;
                temp = tree.Root;
            }
            public void Dispose()
            {
            }
            public void Reset() => pos = -1;
            public bool MoveNext()
            {
                while (stack.Count > 0 || temp != null)
                {
                    if (temp != null)
                    {
                        stack.Push(temp);
                        temp = temp.Left;
                    }
                    else
                    {
                        chekerNode = stack.Peek();
                        if (chekerNode.Right != null && prev != chekerNode.Right)
                            temp = chekerNode.Right;                        
                        else
                        {
                            node = chekerNode;
                            prev = stack.Pop();
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public class MyGraph<T>
        {
            private int Count;
            public Node<T> Root;

            public void Add(T value)
            {
                if (Root == null)
                    Root = new Node<T>(value);            
                else
                    AddTo(Root, value);               
                Count++;
            }
            public int Get_Count() => Count;
            public void AddTo(Node<T> node, T value)
            {
                if (node.Left != null && node.Right != null)
                {
                    Random rnd = new();
                    int rndval = rnd.Next(0, 2);
                    if (rndval == 0)
                        AddTo(node.Left, value);                    
                    else
                        AddTo(node.Right, value);                   
                }
                else if (node.Left == null)
                {
                    node.Left = new Node<T>(value);
                    return;
                }
                else
                {
                    node.Right = new Node<T>(value);
                    return;
                }
            }

            public Selector<T> TreeTraversal(TreeTraversalType treeTraversalType)
            {
                Selector<T> enumer = new(treeTraversalType, this);
                return enumer;
            }
        }
        public class Node<T>
        {
            public Node<T> Left;
            public Node<T> Right;
            public T data;

            public Node(T value) => data = value;
            public override string ToString() => $"{this.data}";
        }

        static void Main(string[] args)
        {
            MyGraph<int> graph = new();
            graph.Add(1);
            graph.Add(2);
            graph.Add(3);
            graph.Add(4);
            graph.Add(5);
            graph.Add(6);
            graph.Add(7);
            graph.Add(8);

            foreach (Node<int> n in graph.TreeTraversal(TreeTraversalType.LevelOrder))
                Console.WriteLine(n);           
        }
    }
}
