using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DataFusion.UserControls
{
    public class AnimationPath : Shape
    {
        /// <summary>
        ///     故事板
        /// </summary>
        private Storyboard _storyboard;

        /// <summary>
        ///     路径长度
        /// </summary>
        private double _pathLength;

        /// <summary>
        ///     路径
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data),
            typeof(Geometry), typeof(AnimationPath), new FrameworkPropertyMetadata(null,
                OnPropertiesChanged));

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimationPath path)
            {
                path.UpdatePath();
            }
        }

        /// <summary>
        ///     路径
        /// </summary>
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

        /// <summary>
        ///     路径长度
        /// </summary>
        public static readonly DependencyProperty PathLengthProperty = DependencyProperty.Register(
            "PathLength", typeof(double), typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnPropertiesChanged));

        /// <summary>
        ///     路径长度
        /// </summary>
        public double PathLength
        {
            get => (double)GetValue(PathLengthProperty);
            set => SetValue(PathLengthProperty, value);
        }

        /// <summary>
        ///     动画间隔时间
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration", typeof(Duration), typeof(AnimationPath), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)),
                OnPropertiesChanged));

        /// <summary>
        ///     动画间隔时间
        /// </summary>
        public Duration Duration
        {
            get => (Duration)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            "IsPlaying", typeof(bool), typeof(AnimationPath), new FrameworkPropertyMetadata(true, (o, args) =>
            {
                var ctl = (AnimationPath)o;
                var v = (bool)args.NewValue;
                if (v)
                {
                    ctl.UpdatePath();
                }
                else
                {
                    ctl._storyboard?.Pause();
                }
            }));

        /// <summary>
        ///     是否正在播放动画
        /// </summary>
        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        public static readonly DependencyProperty RepeatBehaviorProperty = DependencyProperty.Register(
            "RepeatBehavior", typeof(RepeatBehavior), typeof(AnimationPath), new PropertyMetadata(RepeatBehavior.Forever));

        /// <summary>
        ///     动画重复行为
        /// </summary>
        public RepeatBehavior RepeatBehavior
        {
            get => (RepeatBehavior)GetValue(RepeatBehaviorProperty);
            set => SetValue(RepeatBehaviorProperty, value);
        }

        static AnimationPath()
        {
            StretchProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(Stretch.Uniform,
                OnPropertiesChanged));

            StrokeThicknessProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double1Box,
                OnPropertiesChanged));
        }

        public AnimationPath() => Loaded += (s, e) => UpdatePath();

        /// <summary>
        ///     动画完成事件
        /// </summary>
        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(AnimationPath));

        /// <summary>
        ///     动画完成事件
        /// </summary>
        public event EventHandler Completed
        {
            add => AddHandler(CompletedEvent, value);
            remove => RemoveHandler(CompletedEvent, value);
        }

        /// <summary>
        ///     更新路径
        /// </summary>
        private void UpdatePath()
        {
            if (!Duration.HasTimeSpan || !IsPlaying) return;

            _pathLength = PathLength > 0 ? PathLength : Data.GetTotalLength(new Size(ActualWidth, ActualHeight), StrokeThickness);

            if (Math.Abs(_pathLength) < 1E-06) return;

            StrokeDashOffset = _pathLength;
            StrokeDashArray = new DoubleCollection(new List<double>
            {
                _pathLength,
                _pathLength
            });

            //定义动画
            if (_storyboard != null)
            {
                _storyboard.Stop();
                _storyboard.Completed -= Storyboard_Completed;
            }
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior
            };
            _storyboard.Completed += Storyboard_Completed;

            var frames = new DoubleAnimationUsingKeyFrames();
            //开始位置
            var frame0 = new LinearDoubleKeyFrame
            {
                Value = _pathLength,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero)
            };
            //结束位置
            var frame1 = new LinearDoubleKeyFrame
            {
                Value = -_pathLength,
                KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan)
            };
            frames.KeyFrames.Add(frame0);
            frames.KeyFrames.Add(frame1);

            Storyboard.SetTarget(frames, this);
            Storyboard.SetTargetProperty(frames, new PropertyPath(StrokeDashOffsetProperty));
            _storyboard.Children.Add(frames);

            _storyboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e) => RaiseEvent(new RoutedEventArgs(CompletedEvent));
    }

    internal static class ValueBoxes
    {
        internal static object TrueBox = true;

        internal static object FalseBox = false;

        internal static object Double0Box = .0;

        internal static object Double01Box = .1;

        internal static object Double1Box = 1.0;

        internal static object Double10Box = 10.0;

        internal static object Double20Box = 20.0;

        internal static object Double100Box = 100.0;

        internal static object Double200Box = 200.0;

        internal static object Double300Box = 300.0;

        internal static object DoubleNeg1Box = -1.0;

        internal static object Int0Box = 0;

        internal static object Int1Box = 1;

        internal static object Int2Box = 2;

        internal static object Int5Box = 5;

        internal static object Int99Box = 99;

        internal static object BooleanBox(bool value) => value ? TrueBox : FalseBox;
    }

    public static class GeometryExtension
    {
        /// <summary>
        ///     获取路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static double GetTotalLength(this Geometry geometry)
        {
            if (geometry == null) return 0;

            var pathGeometry = PathGeometry.CreateFromGeometry(geometry);
            pathGeometry.GetPointAtFractionLength(1e-4, out var point, out _);
            var length = (pathGeometry.Figures[0].StartPoint - point).Length * 1e+4;

            return length;
        }

        /// <summary>
        ///     获取路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="size"></param>
        /// <param name="strokeThickness"></param>
        /// <returns></returns>
        public static double GetTotalLength(this Geometry geometry, Size size, double strokeThickness = 1)
        {
            if (geometry == null) return 0;

            if (Math.Abs(size.Width) < 1E-06 || Math.Abs(size.Height) < 1E-06) return 0;

            var length = GetTotalLength(geometry);
            var sw = geometry.Bounds.Width / size.Width;
            var sh = geometry.Bounds.Height / size.Height;
            var min = Math.Min(sw, sh);

            if (Math.Abs(min) < 1E-06 || Math.Abs(strokeThickness) < 1E-06) return 0;

            length /= min;
            return length / strokeThickness;
        }
    }
}
