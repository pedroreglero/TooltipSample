using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("CrossGeeks")]
[assembly: ExportEffect(typeof(iOSTooltipEffect), nameof(TooltipEffect))]
namespace ToolTipSample.iOS.Effects
{
    public class iOSTooltipEffect : PlatformEffect
    {

        EasyTipView.EasyTipView tooltip;
        ICommand command;

        public iOSTooltipEffect()
        {
            tooltip = new EasyTipView.EasyTipView();
            tooltip.DidDismiss += OnDismiss;
        }


        void OnDismiss(object sender, EventArgs e)
        {
            if (command != null)
                command.Execute(null);

            if (tooltip != null)
                tooltip.Dispose();
        }


        protected override void OnAttached()
        {
            var control = Control ?? Container;

            var text = TooltipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                tooltip.BubbleColor = TooltipEffect.GetBackgroundColor(Element).ToUIColor();
                tooltip.ForegroundColor = TooltipEffect.GetTextColor(Element).ToUIColor();
                tooltip.Text = new Foundation.NSString(text);
                command = TooltipEffect.GetDismishedCommand(Element);
                UpdatePosition();

                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }


                tooltip?.Show(control, vc.View, true);
            }
        }

        protected override void OnDetached()
        {
            tooltip?.Dismiss();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == TooltipEffect.BackgroundColorProperty.PropertyName)
            {
                tooltip.BubbleColor = TooltipEffect.GetBackgroundColor(Element).ToUIColor();
            }
            else if (args.PropertyName == TooltipEffect.TextColorProperty.PropertyName)
            {
                tooltip.ForegroundColor = TooltipEffect.GetTextColor(Element).ToUIColor();
            }
            else if (args.PropertyName == TooltipEffect.TextProperty.PropertyName)
            {
                tooltip.Text = new Foundation.NSString(TooltipEffect.GetText(Element));
            }
            else if (args.PropertyName == TooltipEffect.PositionProperty.PropertyName)
            {
                UpdatePosition();
            }
        }

        void UpdatePosition()
        {
            var position = TooltipEffect.GetPosition(Element);
            switch (position)
            {
                case TooltipPosition.Top:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Bottom;
                    break;
                case TooltipPosition.Left:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Right;
                    break;
                case TooltipPosition.Right:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Left;
                    break;
                default:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    break;
            }
        }
    }
}