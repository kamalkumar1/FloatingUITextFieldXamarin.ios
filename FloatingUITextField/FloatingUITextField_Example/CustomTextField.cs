using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace FloatingUITextField_Example.iOS
{
    public partial class FloatingUITextField_Example : UITextField
    {
        public FloatingUITextField_Example(IntPtr handle) : base(handle)
        {

        }
        private UILabel m_label;
        private UIView m_lineview;
        private UIColor m_lineColor = new UIColor(0.87f, 0.87f, 0.87f, 1.0f);
        private nfloat bottomhieght;
        public override void Draw(CGRect rect)
        {
            //  m_lineColor = FloatingTextFieldConfig.FLOATING_THEME_COLOR != null ? FloatingTextFieldConfig.FLOATING_THEME_COLOR :  new UIColor(TintColor.CGColor);
            //base.Draw(rect);UIFont.FromName(AppConstant.APP_FONT_NAME_REGULAR, 12);
            Font = FloatingTextFieldConfig.DEFAULT_TEXTFIELD_FONT != null ? FloatingTextFieldConfig.DEFAULT_TEXTFIELD_FONT : Font;
            if (Placeholder != null)
            {
                AttributedPlaceholder = new NSAttributedString(Placeholder, null, new UIColor(red: 0.58f, green: 0.62f, blue: 0.65f, alpha: 1.0f));
            }
            if (m_lineview == null)
            {
                m_lineview = new UIView();
                m_lineview.Frame = new CGRect(0, this.Frame.Height - 1, Frame.Width, 1);
                m_lineview.BackgroundColor = m_lineColor;
                AddSubview(m_lineview);
            }

            TextColor = FloatingTextFieldConfig.DEFAULT_TEXTFIELD_COLOR != null ? FloatingTextFieldConfig.DEFAULT_TEXTFIELD_COLOR : UIColor.Black;
            bottomhieght = (nfloat)(-Math.Abs((Frame.Height / 100) * 20));

            if (Text.Length > 0)
            {
                MovePlaceHolderUp();
            }


        }
        private void InitializeLabel()
        {

            m_label = new UILabel
            {
                Font = FloatingTextFieldConfig.FLOATINGLABEL_FONT != null ? FloatingTextFieldConfig.FLOATINGLABEL_FONT : UIFont.FromName(Font.FamilyName, Font.PointSize - 4),
                Text = Placeholder,
                // Frame = new CGRect(0, 20, rect.Width, 13),
                TextColor = FloatingTextFieldConfig.FLOATING_THEME_COLOR,
                Alpha = 0.0f

            };
            m_label.SizeToFit();
            m_label.Lines = 1;
            AddSubview(m_label);

            Placeholder = Placeholder; // sets up label frame
        }
        public override void AwakeFromNib() => InitializeLabel();
        public override string Placeholder
        {
            get { return base.Placeholder; }
            set
            {
                base.Placeholder = value;

                if (m_label != null)
                {
                    m_label.Text = value;
                    m_label.SizeToFit();

                    m_label.Frame =
                        new CGRect(
                            0, m_label.Font.LineHeight,
                            m_label.Frame.Size.Width, m_label.Frame.Size.Height);
                }
            }
        }
        void MovePlaceHolderUp()
        {
            Animate(0.2, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
            {

                if (m_lineview != null)
                {
                    m_lineview.BackgroundColor = m_lineColor;
                }
                else
                {
                    m_lineview = new UIView
                    {
                        Frame = new CGRect(0, Frame.Height - 1, Frame.Width, 1)
                    };
                    this.AddSubview(m_lineview);
                }


                m_label.Frame =
                    new CGRect(
                        m_label.Frame.Location.X,
                        0.0f,
                        this.Frame.Width,
                        m_label.Frame.Size.Height);
                m_label.Alpha = 1.0f;
                m_lineview.BackgroundColor = FloatingTextFieldConfig.FLOATING_THEME_COLOR != null ? FloatingTextFieldConfig.FLOATING_THEME_COLOR : m_lineColor;

            }, null
                       );
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (string.IsNullOrEmpty(Text))
            {
                if (m_lineview != null)
                {

                    Animate(0.2, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
                    {

                        m_lineview.BackgroundColor = m_lineColor;
                        m_lineview.Frame = new CGRect(0, Frame.Height - 1, Frame.Width, 1);

                        m_label.Frame =
                                    new CGRect(
                                        m_label.Frame.Location.X,
                                        m_label.Font.LineHeight + 2,
                                        this.Frame.Width,
                                        m_label.Frame.Size.Height);
                        m_label.Alpha = 0.0f;
                    }, null);
                }
            }

            else
            {

                if (Text.Length <= 1)
                {
                    MovePlaceHolderUp();
                }
                else if (m_lineview != null)
                {
                    m_lineview.BackgroundColor = FloatingTextFieldConfig.FLOATING_THEME_COLOR != null ? FloatingTextFieldConfig.FLOATING_THEME_COLOR : m_lineColor;
                }

            }
        }

        public override CGRect ClearButtonRect(CGRect forBounds)
        {
            var rect = base.ClearButtonRect(forBounds);

            //if (_floatingLabel == null)
            //    return rect;

            return new CGRect(
                rect.X, rect.Y + m_label.Font.LineHeight / 2.0f,
                rect.Size.Width, rect.Size.Height);
        }

        public override CGRect TextRect(CGRect forBounds)
        {

            var edgeInsets = new UIEdgeInsets(0, 2, bottomhieght, 15);
            return edgeInsets.InsetRect(forBounds);
        }
        public override CGRect EditingRect(CGRect forBounds)
        {
            var edgeInsets = new UIEdgeInsets(0, 2, bottomhieght, 15);
            return edgeInsets.InsetRect(forBounds);
        }

        public override CGRect PlaceholderRect(CGRect forBounds)
        {


            var edgeInsets = new UIEdgeInsets(0, 2, bottomhieght, 15);
            return edgeInsets.InsetRect(forBounds);
        }
    }
}