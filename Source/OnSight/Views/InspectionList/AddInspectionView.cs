﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

namespace OnSight
{
	public class AddInspectionView : BaseContentView<InspectionListViewModel>
	{
		#region Constant Fields
		readonly Entry _titleEntry;
		readonly BoxView _backgroundOverlayBoxView;
		readonly Frame _overlayFrame;
		readonly StackLayout _textEntryButtonStack;

		readonly RelativeLayout _relativeLayout;

		readonly Color _whiteWith75Opacity = new Color(255, 255, 255, 0.75);
		readonly Color _blackWith75PercentOpacity = new Color(0, 0, 0, 0.75);
		#endregion

		public AddInspectionView()
		{
			const string titleText = "New Inspection";
			const string submitButtonText = "Submit";

			_backgroundOverlayBoxView = new BoxView
			{
				BackgroundColor = _whiteWith75Opacity
			};
			_backgroundOverlayBoxView.Opacity = 0;

			var titleLabel = new Label
			{
				FontAttributes = FontAttributes.Bold,
				Text = titleText,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			_titleEntry = new Entry
			{
				Placeholder = "Title"
			};
			_titleEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.TitleEntryText));

			var submitButton = new Button
			{
				BackgroundColor = _blackWith75PercentOpacity,
				TextColor = Color.White,
				BorderWidth = 1,
				BorderColor = _blackWith75PercentOpacity,
				FontAttributes = FontAttributes.Bold,
				Margin = new Thickness(5),
				Text = submitButtonText
			};
			submitButton.SetBinding(Button.CommandProperty, nameof(ViewModel.SubmitButtonCommand));
			submitButton.Clicked += (sender, e) => DismissView();

			_textEntryButtonStack = new StackLayout
			{
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Spacing = 20,
				Children = {
					titleLabel,
					_titleEntry,
					submitButton
				}
			};
			_textEntryButtonStack.Scale = 0;

			_overlayFrame = new Frame
			{
				HasShadow = true,
				BackgroundColor = Color.White,
                Content = _textEntryButtonStack
			};
			_overlayFrame.Scale = 0;

			_relativeLayout = new RelativeLayout();
			Func<RelativeLayout, double> getOverlayFrameHeight = (p) => _overlayFrame.Measure(p.Width, p.Height).Request.Height;
			Func<RelativeLayout, double> getOverlayFrameWidth = (p) => _overlayFrame.Measure(p.Width, p.Height).Request.Width;

			_relativeLayout.Children.Add(_backgroundOverlayBoxView,
			   	Constraint.Constant(-10),
			   	Constraint.Constant(0),
			  	Constraint.RelativeToParent(parent => parent.Width + 20),
				Constraint.RelativeToParent(parent => parent.Height)
		   	);
			_relativeLayout.Children.Add(_overlayFrame,
				Constraint.RelativeToParent(parent => parent.Width / 2 - getOverlayFrameWidth(parent) / 2 - 25),
                Constraint.RelativeToParent(parent =>parent.Height / 4 - getOverlayFrameHeight(parent) / 2),
				Constraint.RelativeToParent(parent => getOverlayFrameWidth(parent) + 50),
				Constraint.RelativeToParent(parent => getOverlayFrameHeight(parent) + 40)
			);

			Content = _relativeLayout;
		}

		public void DisplayView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var animationList = new List<Task>
				{
					_backgroundOverlayBoxView.FadeTo(1,AnimationConstants.AddInspectionViewAnimationTime),
					_textEntryButtonStack.ScaleTo(AnimationConstants.AddInspectionViewMaxSize, AnimationConstants.AddInspectionViewAnimationTime),
				 	_overlayFrame.ScaleTo(AnimationConstants.AddInspectionViewMaxSize,AnimationConstants.AddInspectionViewAnimationTime)
				};
				await Task.WhenAll(animationList);

				animationList = new List<Task>
				{
					_textEntryButtonStack.ScaleTo(AnimationConstants.AddInspectionViewNormalSize, AnimationConstants.AddInspectionViewAnimationTime),
					_overlayFrame.ScaleTo(AnimationConstants.AddInspectionViewNormalSize, AnimationConstants.AddInspectionViewAnimationTime)
				};
				await Task.WhenAll(animationList);

				_titleEntry.Focus();
			});
		}

		public void DismissView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await this.FadeTo(0);
				IsVisible = false;
				InputTransparent = true;
			});
		}
	}
}


