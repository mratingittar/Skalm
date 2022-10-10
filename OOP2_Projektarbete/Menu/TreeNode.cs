using System.Collections;

namespace Skalm.Menu
{
    internal class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        #region PROPERTIES
        public T Value { get; private set; }
        public TreeNode<T>? Parent { get; private set; }
        public List<TreeNode<T>> Children { get; private set; }
        public bool IsRoot => Parent == null;
        public bool IsLeaf => Children == null;
        public int Depth => IsRoot ? 0 : Parent!.Depth + 1;
        private ICollection<TreeNode<T>> ElementsIndex { get; set; }
        #endregion

        #region CONSTRUCTOR
        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
        }

        public TreeNode(T value, params T[] childValues)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
            ElementsIndex = new LinkedList<TreeNode<T>>();
            ElementsIndex.Add(this);
            AddChildren(childValues);
        }
        #endregion

        #region NODE CHILDREN
        public TreeNode<T> AddChild(T value)
        {
            TreeNode<T> node = new TreeNode<T>(value) { Parent = this };
            Children.Add(node);
            RegisterChildrenForSearch(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        private void RegisterChildrenForSearch(TreeNode<T> node)
        {
            ElementsIndex.Add(node);
            if (Parent != null)
                Parent.RegisterChildrenForSearch(node);
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            ElementsIndex.Remove(node);
            return Children.Remove(node);
        } 
        #endregion

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (TreeNode<T> child in Children)
                child.Traverse(action);
        }

        public TreeNode<T> FindNode(Func<TreeNode<T>, bool> predicate)
        {
            return ElementsIndex.FirstOrDefault(predicate)!;
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(Children.SelectMany(x => x.Flatten()));
        }

        #region ITERATING
        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            yield return this;
            foreach (TreeNode<T> directChild in Children)
            {
                foreach (TreeNode<T> child in directChild)
                {
                    yield return child;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
        #endregion
    }
}

// https://github.com/gt4dev/yet-another-tree-structure/blob/master/csharp/CSharpTree/TreeNode.cs
