using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Material.Icons;
using System.Diagnostics.CodeAnalysis;

namespace Sy.Avalonia.Material.Controls;

[PseudoClasses(PcElevated, PcHeadlined)]
[TemplatePart(PART_Actions, typeof(StackPanel))]
[TemplatePart(PART_Headline, typeof(ContentPresenter))]
[TemplatePart(PART_Title,	 typeof(ContentPresenter))]
public sealed class TopAppBar : TemplatedControl {
	private const string PcElevated = "elevated";
	private const string PcHeadlined = "headlined";
	private const string PART_Actions = "PART_Actions";
	private const string PART_Headline = "PART_Headline";
	private const string PART_Title = "PART_Title";

	private readonly TopAppBarActionCollection _actions = new();

	private bool _hasAllComponents;

	private double _contentOffset;
	private double _heightOffset;
	private double _heightOffsetLimit;
	private double _startedPullingDownAtOffset;

	private ContentPresenter? _title;
	private TranslateTransform? _headlineTransform;

	/// <summary>
	/// Defines the <see cref="Actions"/> property.
	/// </summary>
	public static readonly DirectProperty<TopAppBar, TopAppBarActionCollection> ActionsProperty =
		AvaloniaProperty.RegisterDirect<TopAppBar, TopAppBarActionCollection>(
			nameof(Actions)
			, o => o.Actions);

	/// <summary>
	/// Defines the <see cref="IsElevateEnabled"/> property
	/// </summary>
	public static readonly StyledProperty<bool> IsElevateEnabledProperty =
		AvaloniaProperty.Register<TopAppBar, bool>(nameof(IsElevateEnabled), true);

	/// <summary>
	/// Defines the <see cref="Title"/> property.
	/// </summary>
	public static readonly StyledProperty<object?> TitleProperty =
		AvaloniaProperty.Register<TopAppBar, object?>(nameof(Title), string.Empty);


	/// <summary>
	/// Defines the <see cref="TitleTheme"/> property.
	/// </summary>
	public static readonly StyledProperty<ControlTheme?> TitleThemeProperty =
		AvaloniaProperty.Register<TopAppBar, ControlTheme?>(nameof(TitleTheme));


	/// <summary>
	/// Gets the collection of action controls.
	/// </summary>
	public TopAppBarActionCollection Actions {
		get => _actions;
	}

	/// <summary>
	/// Gets the total offset of the content scrolled under the control.
	/// </summary>
	public double ContentOffset {
		get;
	}

	/// <summary>
	/// Gets of sets the <see cref="ControlTheme"/> applied to the <see cref="Title"/> content 
	/// when it is featured as a headline.
	/// </summary>
	public ControlTheme? HeadlinedTitleTheme {
		get;
		set;
	}

	/// <summary>
	/// Gets the height offset.
	/// </summary>
	public double HeightOffset {
		get;
	}

	/// <summary>
	/// Gets the height offset limit, which represents the limit that the control is able to 
	/// collapse to.
	/// </summary>
	public double HeightOffsetLimit {
		get;
	}

	/// <summary>
	/// Gets or sets the horizontal alignment of the background image.
	/// </summary>
	public HorizontalAlignment HorizontalImageAlignment {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the background image.
	/// </summary>
	public Image? Image {
		get;
		set;
	}

	/// <summary>
	/// Gets whether the control is elevated due to content being scrolled behind it.
	/// </summary>
	public bool IsElevated {
		get;
	}

	/// <summary>
	/// Gets or sets whether the control elevates when content is scrolled behind it.
	/// </summary>
	public bool IsElevateEnabled {
		get => GetValue(IsElevateEnabledProperty);
		set => SetValue(IsElevateEnabledProperty, value);
	}

	/// <summary>
	/// Gets or sets the image displayed for the navigation icon.
	/// </summary>
	public MaterialIconKind NavigationIcon {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets how the control responds to the inner content of a <see cref="Scaffold"/> 
	/// being scrolled.
	/// </summary>
	public TopAppBarScrollBehavior ScrollBehavior {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the title.
	/// </summary>
	public object? Title {
		get => GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

	/// <summary>
	/// Gets or sets the <see cref="ControlTheme"/> applied to the <see cref="Title"/> content.
	/// </summary>
	public ControlTheme? TitleTheme {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the vertical alignment of the background image.
	/// </summary>
	public VerticalAlignment VerticalImageAlignment {
		get;
		set;
	}

	/// <summary>
	/// Creates a new <see cref="TopAppBar"/> instance.
	/// </summary>
	public TopAppBar() {
		
	}

	protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
		base.OnApplyTemplate(e);

		//var title = e.NameScope.FindRequired<ContentPresenter>(PART_Title);
		//var headline = e.NameScope.FindRequired<ContentPresenter>(PART_Headline);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
		base.OnPropertyChanged(change);

		if (change.Property == IsElevateEnabledProperty) {
			UpdateElevation();
			return;
		}
	}

	[MemberNotNullWhen(true, nameof(_title))]
	[MemberNotNullWhen(true, nameof(_headlineTransform))]
	private bool CheckParts() {
		return _hasAllComponents;
	}

	private void Headline_SizeChanged(object? sender, SizeChangedEventArgs e) {
		
	}

	private void UpdateElevation() {
		
	}

	private void UpdateMargins() {
		
	}

	private void UpdateOpacities() {
		
	}

	private void UpdateTransforms() {
		
	}
}
