   M S B A M L   `   `   `  ����   GraduateWork updated*    $GraduateWork_updated.WindowReference   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Справка��$	��700��$	��550��$	��450��$	��400�� � AllowsTransparency .  � Icon$ img/icon_app.png�� � 
ResizeMode$ CanResizeWithGrip=� � WindowStartupLocation$ Manual=� � WindowStyle$
 None=�����  �� ImageSource$ img/wallpaper.jpg��.���� ���	#FF034F84$��1q�._�� -    �Name$ grid_mainContainer�� � RowDefinitions �� $
��auto ��� $	��18* �.+��� -   	 ��Name$	 leftSizeGrip��$��5��
 K� ZIndex$
 1��$��0��$��2�� � Cursor$ SizeWE��$
��Left=�   RectBorderStyle#��[  �� -   $	 rightSizeGrip��$��5��$��Right=�$
 1��$��0��$��2��$ SizeWE��#��[  �� -   $	 topSizeGrip��$��5��$
 1��$	��Top=�$ SizeNS��#��[  �� -   $	 bottomSizeGrip��$��1��$��5��$
 1��$��Bottom=�$ SizeNS��#��[  �� -   $��topLeftSizeGrip��$��5��$��5��$
 1��$��0��$
��Left=�$	��Top=�$ SizeNWSE��#��[  �� -   $��bottomRightSizeGrip��$��5��$��5��$
 1��$��1��$��Right=�$��Bottom=�$ SizeNWSE��#��[  �� -	   $��topRightSizeGrip��$��5��$��5��$
 1��$��0��$��Right=�$	��Top=�$ SizeNESW��#��[  �� -
   $��bottomLeftSizeGrip��$��5��$��5��$
 1��$��1��$
��Left=�$��Bottom=�$ SizeNESW��#��[  ]� -    ]�Name$ 
stackPanel��$��0�����	#FF034F84.+��� $	��5 0q�$��17��$��img/icon_app.png���� $��Center=�$��Справка��$��Calibri)�$��18(�
�������$��5 0 0 0q�o� $��Right=���Z� .@�m� -    m�Name$ btnMinimizeWindow��$��0q�$
 Hand��$��0q� � ToolTip$ Свернуть окно�� m� Icon �� $��img/substract.png��m� -   $ btnRestore_window��$
��auto��$
 Hand��  top_menu_tabs#��[ $ Полный экран�� �� $��img/restore.png��m� -   $ btnClose_window��$��0 0 0 0q�$
 Hand��$��0q�$ Закрыть окно�� �� $��img/cross.png���� $��1��$��
10 5 10 10q�$��14(�.@��� -    ��Name$ tab_aboutInput������  �� Orientation$ 
Horizontal=�.+��� $��16��$��2 4 4 2q�$#��img/ReferenceWindow/input.png���� $��Center=�Входные данные.��.� .��/�  /� ColumnWidth$	 400�� /� IsHyphenationEnabled .   /� IsOptimalParagraphEnabled . .T�J� $��18(�$
��Bold"�.*�ПользовательJ� $��16(� �� TextAlignment$ Justify=� J� 
TextIndent$ 30��.*�.*��� -    �Name$ prghAboutUserInput��J� $��18(�$
��Bold"�.*�ГенерацияJ� $��16(�$ Justify=�$ 30��.*�.*��� -   $ prghAboutGenerationInput��J� $��18(�$
��Bold"�.*�
ФайлJ� $��16(�$ Justify=�$ 30��.*�.*��� -   $ prghAboutFileInput���� -   $ tab_aboutAlgorithm������ $ 
Horizontal=�.+��� $��16��$��2 4 4 2q�$'��!img/ReferenceWindow/algorithm.png���� $��Center=�Алгоритмы.��.� .��/� $	 400�� .  . .T��� $��16(�.�J� $ Justify=�$ 30��.*�.*��� -   $ prghAboutAlgorithms���� $��18(�$
��Bold"�.�J� .*�ОднопоточныйJ� $��16(�$ Justify=�$ 30��.*�.*��� -   $! prghSingleThreadedAlgorithm��J� $��18(�$
��Bold"�.*�МногопоточныйJ� $��16(�$ Justify=�$ 30��.*�.*��� -   $  prghMultiThreadedAlgorithm��J� $��18(�$
��Bold"�.*�Форд-ФалкерсонJ� $��16(�$ Justify=�$ 30��.*�.*��� -   $  prghFordFulkersonAlgorithm���� -   $ tab_aboutProgram������ $ 
Horizontal=�.+��� $��16��$��2 4 4 2q�$*��$img/ReferenceWindow/aboutProgram.png���� $��Center=�О программе.��.� .��/� .T�J� $��16(�$ Justify=�$ 30��.*�.*��� -   $ prghAboutProgram��