   M S B A M L   `   `   `  ���� O  KGraduateWork updated, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:GraduateWork_updatedGraduateWork_updated  2    ,GraduateWork_updated.WindowRenderingOfResult   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:GraduateWork_updated     -   $	��550��$	��390��$	��540��$	��390��  � AllowsTransparency  .  � Icon$ img/icon_app.png�� � 
ResizeMode$ CanResizeWithGrip=�   CustomWindow#��[  ����  �� ImageSource$ img/wallpaper.jpg��.��� -    �Name$ grid_mainContainer�� � RowDefinitions �� $
��auto ��� $��* �.+��� -    ��Name$ leftSizeGrip��$��5�� K� ZIndex$ 1��$��0��$��2�� � Cursor$ SizeWE��$
��Left=�  RectBorderStyle#��[ �� -   $ rightSizeGrip��$��5��$��Right=�$ 1��$��0��$��2��$ SizeWE��#��[ �� -   $ topSizeGrip��$��5��$ 1��$	��Top=�$ SizeNS��#��[ �� -   $ bottomSizeGrip��$��1��$��5��$ 1��$��Bottom=�$ SizeNS��#��[ �� -   $��topLeftSizeGrip��$��5��$��5��$ 1��$��0��$
��Left=�$	��Top=�$ SizeNWSE��#��[ �� -   $��bottomRightSizeGrip��$��5��$��5��$ 1��$��2��$��Right=�$��Bottom=�$ SizeNWSE��#��[ �� -	   $��topRightSizeGrip��$��5��$��5��$ 1��$��0��$��Right=�$	��Top=�$ SizeNESW��#��[ �� -
   $��bottomLeftSizeGrip��$��5��$��5��$ 1��$��1��$
��Left=�$��Bottom=�$ SizeNESW��#��[ �� $��0��$��2��$��1q����	#FF034F84]� -   $��top_docPanel��$��0������ 	 �� 
StartPoint$
	 0, 1&�
 �� EndPoint$

 1, 0&�.���  � Offset$ 0.603Q� � Color$ 	#FF034F84��.+��� $	��5 0q�$��17��$��img/icon_app.png���� -    ��Name$ lbl_topPanel��$��Center=�$��Тест��$��Calibri)�$��18(�
�������$��5 0 0 0q�o� -   $��top_menu_btn��$��Right=���Z� .@�m� -   $��btnMinimize_window��$
��auto��$
 Hand��  top_menu_tabs#��[  � ToolTip$ Свернуть окно�� m� Icon �� $��img/substract.png��m� -   $��btnRestore_window��$
��auto��$
 Hand��#��[ $ Полный экран�� �� $��img/restore.png��m� -   $��btnClose_window��$
 Hand��#��[ $) #Закрыть приложение�� �� $��img/cross.png��� -   $ grid_rendering��$��1�� � ColumnDefinitions �� $
��0.2* ��� $��13.2* ��� $
��0.2* � �� $
��0.5* ��� $	��24* ��� $
��0.5* �