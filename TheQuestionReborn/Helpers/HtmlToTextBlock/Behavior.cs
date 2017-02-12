using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace TheQuestionReborn.Helpers.HtmlToTextBlock
{
    public abstract class Behavior<T> : Behavior where T : DependencyObject
    {
        protected new T AssociatedObject => base.AssociatedObject as T;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (this.AssociatedObject == null) throw new InvalidOperationException("AssociatedObject is not of the right type");
        }
    }

    public abstract class Behavior : DependencyObject, IBehavior
    {
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            OnAttached();
        }

        public void Detach()
        {
            OnDetaching();
        }

        protected virtual void OnAttached() { }

        protected virtual void OnDetaching() { }

        protected DependencyObject AssociatedObject { get; set; }

        DependencyObject IBehavior.AssociatedObject => AssociatedObject;
    }
}
