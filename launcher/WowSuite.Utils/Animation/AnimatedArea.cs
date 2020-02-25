using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WowSuite.Launcher.Animation.UiElements
{
    public class AnimatedArea : Border
    {
        #region Fields

        private ImageBrush[] _brushes;
        private Int32Animation _animation;

        private Duration _duration;

        private bool _isForeverAnimationSubscribed;
        private bool _isReversibleAnimationSubscribed;


        #endregion

        /// <summary>ѕроисходит, когда анимаци€ заканчиваетс€</summary>
        public event EventHandler ReversibleAnimationCompleted;

        protected virtual void OnReversibleAnimationCompleted()
        {
            EventHandler handler = ReversibleAnimationCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        //dp PROPERTY { get; set; }

        #region dp FrameIndex { get; set; }

        protected static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register("FrameIndex", typeof(int), typeof(AnimatedArea),
                new FrameworkPropertyMetadata(0, ChangingFrameIndex));

        protected static void ChangingFrameIndex(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = ((AnimatedArea)d);
            image.Background = image._brushes[(int)e.NewValue];
        }

        #endregion

        #region dp Frames { get; set; }

        public static readonly DependencyProperty FramesProperty =
            DependencyProperty.Register("Frames", typeof(ImageBrush[]), typeof(AnimatedArea),
                new FrameworkPropertyMetadata(null, ChangingFrames));

        private static void ChangingFrames(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = (AnimatedArea)d;

            image.StopAnimation();

            image._brushes = e.NewValue as ImageBrush[];
            if (image._brushes != null && image.DisplayFirstFrame)
            {
                image.Background = image._brushes[0];
                image.UpdateLayout();
            }
        }

        #endregion

        #region dp DisplayFirstFrame

        public static readonly DependencyProperty DisplayFirstFrameProperty = DependencyProperty.Register(
            "DisplayFirstFrame", typeof(bool), typeof(AnimatedArea), new PropertyMetadata(true));

        #endregion

        #region Properties

        public bool IsPlaying { get; protected set; }


        /// <summary>
        /// ќпредел€ет или задаЄт показывать ли первый кадр анимации, когда кадры присвоены свойству <c>Frame</c>
        /// </summary>
        public bool DisplayFirstFrame
        {
            get { return (bool)GetValue(DisplayFirstFrameProperty); }
            set { SetValue(DisplayFirstFrameProperty, value); }
        }

        public ImageBrush[] Frames
        {
            get { return (ImageBrush[])GetValue(FramesProperty); }
            set { SetValue(FramesProperty, value); }
        }

        public int FrameIndex
        {
            get { return (int)GetValue(FrameIndexProperty); }
            protected set { SetValue(FrameIndexProperty, value); }
        }

        #endregion

        #region Methods

        private void TryUnsuscribeAll()
        {
            if (_isForeverAnimationSubscribed)
            {
                _animation.Completed -= ForeverAnimationWithPause_Completed;
                _isForeverAnimationSubscribed = false;
            }

            if (_isReversibleAnimationSubscribed)
            {
                _animation.Completed -= ReversibleAnimation_Completed;
                _isReversibleAnimationSubscribed = false;
            }
        }

        //ѕосле паузы анимаци€ доходит до конца и запускаетс€ снова в нормальном режиме 
        //(после паузы меньше Duration задаЄтс€, чтобы доиграть анимацию с той же скоростью
        protected void ForeverAnimationWithPause_Completed(object sender, EventArgs e)
        {
            var animation = (AnimationClock)sender;
            animation.Completed -= ForeverAnimationWithPause_Completed;
            _isForeverAnimationSubscribed = false;
            _animation = new Int32Animation(0, _brushes.Length - 1, _duration);
            _animation.RepeatBehavior = RepeatBehavior.Forever;
            BeginAnimation(FrameIndexProperty, _animation);
        }

        public void InitImages(ImageBrush[] pictures)
        {
            if (pictures == null)
                throw new ArgumentNullException("pictures");

            _brushes = pictures;
            StopAnimation();
        }

        /// <summary>
        /// ѕолучить пропорциональное врем€ на проигрывание части кадров от их общего числа. 
        /// Ќужно, чтобы анимаци€, котора€ начинаетс€ не с нулвеого кадра или заканчиваетс€ не на последнем кадре
        /// играла с той же скоростью, как если бы еЄ запустили от нулевого кадра и до последнего
        /// </summary>
        /// <param name="fullDuration"></param>
        /// <param name="quantity"> ол-во кадров, которые нужно проиграть</param>
        /// <returns></returns>
        private Duration GetDurationPerFrames(Duration fullDuration, int quantity)
        {
            long ticksPerFrame = fullDuration.TimeSpan.Ticks / _brushes.LongLength;
            return new Duration(new TimeSpan(ticksPerFrame * quantity));
        }

        public void BeginForeverAnimationWithPause(Duration duration, bool pause = false)
        {
            TryUnsuscribeAll();

            if (pause)
            {
                _animation = new Int32Animation(FrameIndex, FrameIndex, duration);
                _animation.RepeatBehavior = new RepeatBehavior(0d);
            }
            else
            {
                _duration = duration;

                Duration restDuration = GetDurationPerFrames(_duration, _brushes.Length - 1 - FrameIndex);

                _animation = new Int32Animation(FrameIndex, _brushes.Length - 1, FrameIndex == 0 ? duration : restDuration);
                _animation.Completed += ForeverAnimationWithPause_Completed;
                _isForeverAnimationSubscribed = true;
            }

            BeginAnimation(FrameIndexProperty, _animation);
        }

        public void BeginReversibleAnimation(Duration duration, bool reverse = false, bool autoReverse = false)
        {
            if (!reverse)
                BeginReversibleAnimation(duration, new RepeatBehavior(1d), 0, _brushes.Length - 1, reverse, autoReverse);
            else
                BeginReversibleAnimation(duration, new RepeatBehavior(1d), _brushes.Length - 1, 0, reverse, autoReverse);
        }

        /// <summary>
        /// »грать покадровую анимацию. 
        /// ≈сли reverse == false, то startIndex должен быть меньше, чем endIndex.
        /// ≈сли reverse == true, то startIndex должен быть больше, чем endIndex.
        /// </summary>
        /// <param name="duration">ѕолное врем€, за которое должна играть полна€анимци€ от нулевого кадра до последнего</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="reverse"></param>
        /// <param name="autoReverse"></param>
        public void BeginReversibleAnimation(Duration duration, int startIndex, int endIndex, bool reverse = false, bool autoReverse = false)
        {
            BeginReversibleAnimation(duration, new RepeatBehavior(1d), startIndex, endIndex, reverse, autoReverse);
        }

        public void BeginReversibleAnimation(Duration duration, RepeatBehavior repeat, int startIndex, int endIndex, bool reverse = false,
            bool autoReverse = false)
        {
            //if (!reverse && endIndex < startIndex)
            //{
            //    throw new ArgumentOutOfRangeException(string.Format(
            //        "endIndex ({0}) не может быть меньше, чем startIndex ({1})", endIndex, startIndex));
            //}

            IsPlaying = true;
            Duration restDuration = GetDurationPerFrames(duration, !reverse ? endIndex - startIndex : startIndex - endIndex);
            //Duration restDuration = GetDurationPerFrames(duration, !reverse ? endIndex - startIndex : startIndex - endIndex);

            //if (reverse)
            //    _animation = new Int32Animation(endIndex, startIndex, restDuration);
            //else
            _animation = new Int32Animation(startIndex, endIndex, restDuration);

            _animation.AutoReverse = autoReverse;
            _animation.RepeatBehavior = repeat;
            _animation.Completed += ReversibleAnimation_Completed;

            BeginAnimation(FrameIndexProperty, _animation);
        }

        private void ReversibleAnimation_Completed(object sender, EventArgs e)
        {
            var animation = (AnimationClock)sender;
            animation.Completed -= ReversibleAnimation_Completed;
            OnReversibleAnimationCompleted();
        }

        public void StopAnimation()
        {
            TryUnsuscribeAll();
            BeginAnimation(FrameIndexProperty, null);
            FrameIndex = 0;
            IsPlaying = false;
        }

        #endregion
    }
}