   M S B A M L   `   `   `  ����   GraduateWork updated    GraduateWork_updated.App   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       �� 	Resources     RectBorderStyle �� 
TargetType �� Property �� Value �� RelativeSource �� AncestorType  CustomWindow  top_menu_tabs  styleLabel_H1  styleLabel_H2  txtBox_to_label  lblCountList  styleButton �� 
TargetType �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect	   
BlurRadius
   Opacity   ShadowDepth   Color �� Triggers P� Property P� Value �� 
TargetName :� RoutedEvent g� Duration �� 
TargetName �� TargetProperty	 W� To  styleButtonInfo �Name ��Name ��Name	 �� To  	 styleTextBox_sizeInputMatrix 
 styleRadiobutton � Cursor �� Bullet  CheckBoxStyle J� SnapsToDevicePixels  BackgroundColor  BorderColor  ForegroundColor  DisabledColor  DisabledBorderColor  DisabledForegroundColor  MouseOverColor  MouseOverBorderColor  SelectedColor  SelectedBorderColor  InputBorderColor  InputBackgroundColor  DropDownBorderColor  DropDownBackgroundColor  ComboBoxToggleButton � ColumnDefinitions'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState    
Storyboard8  2System.Windows.Media.Animation.EasingColorKeyFrame! �� KeyTime" �� Value# �� Color$ F�Name  ComboBoxTextBox% R� KeyTime  System.Windows.Visibility&   Visible' R� Value(   Hidden) d�Name* �� 	ClickMode+ d� 	IsChecked, �� Mode- �� SelectionBoxItem. �� SelectionBoxItemTemplate/ J� IsHitTestVisible0 ��Name1 �� 
IsReadOnly2 �� 
IsReadOnly3 �Name4 � AllowsTransparency5 �� IsDropDownOpen6 � ActualWidth7 �� MaxDropDownHeight8 K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation9   DirectionalNavigation: �� Padding��%�!  &	        &	 i     &	 �     &	     &	 �    &	 I    &	 �    &	 I    &	 �    &		 �
    &	
     &	 �    &	 6    &	 E    &	 X    &	 k    &	 ~    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	     &	 %    &	 8    7  7  7  7  7  7  7  7  7  7  7  7  &	 �    '��     7  7  7  '�� o    7  7  �� " ��.���  � |� .  ��  � ��
 ���� ��  �@�Tag ��  �� $ WindowL��� " �.���  �@�WindowStyle$
 None=���  � ��
 ���� �� .���  � ��$ Calibri)���  � ��$ 18(���  � ��
 �������  � ��$ 0q��� .���  � ��$ Calibri)���  � ��$ 18(���  � ��$ SemiBold"���  � ��
 �   ���  � ��$ 0q���  � �� �#ffef96��  � ��$
 12 0q��� .���  � ��$ 14(���  � ��$ Center=���  � ��$ Right=���  � ��$ 0q���  � ��$ 0q��� " ��.���  � ��
 ���� ��  � �� Z� ��  � ��$ 0q���  � �� .  ��  � ��$ 14(���  � ��$ Center=���  � ��$
 Bold"��� " ��.���  � ��$ 14(���  � ��$ Center=���  � ��$	 5 0q��� " ��.���  � ��$ 20����  � ��$ 20����  � ��$ 35����  �@J�SnapsToDevicePixels . ��  � �� �� " ��.R��� $��Border�����	#FF034F84
�������$	��1.5q�$ 5��$��Stretch=�#��z��._��� $��Content��$��Center=�$��Center=���. $��14(�
�������   
dropShadow��$	 0Q�$
 0Q�$ 0Q�$ Black�� P�  � {� .  . ��� $ Border�� �@J�Opacity$	 0.4Q�P�  �@J�IsMouseOver . . ���  �@�Cursor$
 Hand��:� $ 
MouseEnter��.[��� .���� .��W� $ 0:0:0.2D�$ 
dropShadow��$ ShadowDepth�$ 1X�W� $ 0:0:0.2D�$ 
dropShadow��$ 
BlurRadius�$ 8X�W� $ 0:0:0.2D�$ 
dropShadow��$ Opacity�$ 1X�:� $ 
MouseLeave��.[��� .���� .��W� $ 0:0:0.1D�$ 
dropShadow��$ ShadowDepth�$ 0X�W� $ 0:0:0.1D�$ 
dropShadow��$ 
BlurRadius�$ 0X�W� $ 0:0:0.2D�$ 
dropShadow��$ Opacity�$ 0X��� " ��.���  � ��$
 Left=���  � ��$ Center=���  � ��
 �   ���  � ��$ 18����  � ��$ 18����  � �� �� " ��.R�� $
 grid��$��18��$��18��$��Right=�$��Center=�.+��� $ border��
����������	#FF034F84$��1q�$ 10��._��� $
 text��$��Center=�$��Center=�$��12(�$
��Bold"� P�  �@J�IsMouseOver . . ���  �@�Cursor$
 Help��:� $ 
MouseEnter��.[��� .���� .���� $ 0:0:0.2D�$ border��$ Background.Color�$ 	#FF034F84X��� $ 0:0:0.2D�$ border��$ BorderBrush.Color�$ WhiteX��� $ 0:0:0.2D�$ Foreground.Color�$ WhiteX�:� $ 
MouseLeave��.[��� .���� .���� $ 0:0:0.2D�$ border��$ Background.Color��� $ 0:0:0.2D�$ border��$ BorderBrush.Color��� $ 0:0:0.2D�$ Foreground.Color�P�  � {� .  . ��� $
 grid�� �@J�Opacity$	 0.4Q��� .���  � ��$ 14(���  � ��$ 30����  � ��$ 5 6 0 3q���  � ��$
 Left=���  � ��$ Center=��� " 	�.���  � �� �� " 	�.R��� $��Center=�$
 Hand�� � $��18��$��18��.+��� $��
RadioOuter��
������ ���	#FF034F84$	��1.5q�$ 2���� $��	RadioMark��$��4q����	#FF034F84$ 0��$y�Hidden=�._��� $��8, 0,0,0q�$��14(�
���   ���  P�  �@d�	IsChecked$
 trueY�. ��� $ 	RadioMark�� � y�$ Visible=��� $ 
RadioOuter�� � �� �#034f84�� $ 
RadioOuter�� � ��
 ������� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $ border��
���   �$��0 0 1 1q�._�� $
��grid��$��1 1 0 0q�
�������$
 Hand�� . .+��� $	��lbl��$
��5, 0q�$��Center=�$��Center=�$��14(� :� $ 
MouseEnter��.[��� .���� .���� $ 0:0:0.2D�$
 grid��$ Background.Color�$ 	#FF034F84X��� $ 0:0:0.2D�$	 lbl��$ Foreground.Color�$ WhiteX��� $ 0:0:0.2D�$ border��$ BorderBrush.Color�$ WhiteX�:� $ 
MouseLeave��.[��� .���� .���� $ 0:0:0.2D�$
 grid��$ Background.Color��� $ 0:0:0.2D�$	 lbl��$ Foreground.Color��� $ 0:0:0.2D�$ border��$ BorderBrush.Color�P�  �@d�	IsChecked$
 TrueY�. ��� $	 lbl�� � ��$ 1���� $
 grid�� � �� �	#FF034F84�� $	 lbl�� � ��
 ������� $ border�� � ��
 �����P�  �@d�	IsChecked$ FalseY�. ��� $	 lbl�� � ��$ 0��P�  � {� .  . ��� $
 grid�� �@J�Opacity$ 1Q�P�  � {� . . ��� $
 grid�� �@J�Opacity$ 1Q���	White����	#FF034F84����	#FF034F84����	#FFF4F4F4����	#FF888888����	#FF888888����	#FFBEE6FD����	#FF034F84����	White����	#FF2C628B����	#FF034F84����	#FFFFFFFF����	#FFABADB3����	#FFFFFFFF���� " d�.R��  �� �� $��20 �   CommonStates��.   
Normal��  	MouseOver��.  �� .���� $ Border��$0 *(Panel.Background).(SolidColorBrush.Color)�.g�  $! 0��8"   �� $ Border��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"  �� $ InputBorder��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"    Pressed��.  �� .���� $ Border��$0 *(Panel.Background).(SolidColorBrush.Color)�.g�  $! 0��8"  �� $ Border��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"  �� $ InputBorder��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"    Disabled��.  �� .���� $ Border��$0 *(Panel.Background).(SolidColorBrush.Color)�.g�  $! 0��8"  �� $ Arrow��$* $(Shape.Fill).(SolidColorBrush.Color)�.g�  $! 0��8"  �� $ Border��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"    CheckStates��.   Checked��.  �� .���� $ Border��$0 *(Panel.Background).(SolidColorBrush.Color)�.g�  $! 0��8" 	 �� $ Border��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8" 
 �� $ InputBorder��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"    	Unchecked��  Indeterminate��.+��� $ Border��$��2��$��2q�$ 4������ ## �  ���� ## �  �� $��Input��$��0��$��0q�$��1,1,0,1q�$ 4,0,0,4������ ## �  ���� ## �  ._��� $��InputBorder��$��0q�$��0,0,1,0q�$ 4,0,0,4������ ## �  ���� ## �  F� $$ Arrow��$��1��$��Center=�$��Center=�"����        � 	=  	= � z     	���� ## �  �� " ��.R��� $ PART_ContentHost��#��z��|�.  �� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��$ 25����  � ��$ 25����  � ��$ 45����  � �� �� " ��.R��    CommonStates��.   
Normal��  	MouseOver��  Disabled��.  �� .���� $ PART_EditableTextBox��$6 0(TextElement.Foreground).(SolidColorBrush.Color)�.g�  $! 0��8"     
EditStates��.   Editable��.  �� .��T� $ PART_EditableTextBox��$ (UIElement.Visibility)�..�i� $% 0��#' Z& T� $ ContentSite��$ (UIElement.Visibility)�..�i� $% 0��#' Z(   
Uneditable��.+�d� $) ToggleButton��$* Press=�|�.  8�� + �� *IsDropDownOpen+$, TwoWay=� �� *TemplatedParent+�� $ ContentSite��$��7,0,10,0q�$
��Left=�$��Center=�#��z- #��z. #��z��/ .  �� $0 PART_EditableTextBox��$��	10,0,10,0q�$
��Left=�$��Center=�
������ |�. #2 z1 8�� $y�Hidden=���Z� � $3 Popup��4 . |�.  #��z5 $��Bottom=�$��Slide=�.��� $ DropDown��#��z6 #��z7  . .+��� $ DropDownBorder��$��0,2,0,0q�$��2q�$ 4������ ## �  ���� ## �  �� $	��4,6q� . .���� 8 . $9 	Contained=� P�  �@��HasItems .  . ��� $ DropDownBorder�� � ��$ 95���� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $ Border��
������ $��2q�$: 4q� . ���� $# Transparent��   SelectionStates��.   Selected��.  �� .���� $ Border��$0 *(Panel.Background).(SolidColorBrush.Color)�.g�  $! 0��8"   �� $ Border��$2 ,(Border.BorderBrush).(SolidColorBrush.Color)�.g�  $! 0��8"    
Unselected��  SelectedUnfocused��._��� 