namespace Skalm.Menu
{
    internal class TreeNode<T>
    {
        public T Value { get; private set; }
        public TreeNode<T>? Parent { get; private set; }
        public List<TreeNode<T>> Children { get; private set; }
        public TreeNode(T value)
        {
            Value = value;
            Children = new List<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T value)
        {
            TreeNode<T> node = new TreeNode<T>(value) { Parent = this };
            Children.Add(node);
            return node;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return Children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (TreeNode<T> child in Children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(Children.SelectMany(x => x.Flatten()));
        }

        public IEnumerable<TreeNode<T>> ReturnNodeWithChildren()
        {
            return new[] {this}.Concat(Children.SelectMany(x => x.ReturnNodeWithChildren()));
        }

    }
}
