   M S B A M L   `   `   `  ����?  ;DeCo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:DeCo.UserControlUIDeCo.UserControlUI  5           DeCo.UserControlUI.MenuBar   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:DeCo.UserControlUI     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      -   5          >�Name$  	ucMenuBar��5	      $��50��5       
  MenuVM � DataContext# [  5      .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ MouseLeftButtonDown��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 5      �� *MouseMoveWindowCommand+   CommandParameter 5      ��  �� ElementName$ 	ucMenuBar��5      5      .��5      � .+�5   
   (  "MaterialDesignThemes.Wpf.ColorZone  
����d�.��5      ]� .+�5      �� 
������ $��Right=�5       �� Orientation$ 
Horizontal=�5      $��Right=�5      	 � 	Resources	 5      
 �� 
TargetType �� Property �� Value��%�   '��       �� 5      "
 ��6    .�5      ��  � ��$ 2,4,2,4q�5    %   �� 5!       � ��$ 40��5"   %   5#      5$      .+�5%      ��    MaterialDesignFlatDarkButton#��[ 5(      ���#a5d6a7��5&      �� *MinimizeWindowCommand+��5'      �� $ 	ucMenuBar��.��5*      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ WindowMinimize=�62   
�������5+   2   5,      �� 5-      #��[ 50      ���#a5d6a7��5.      �� *MaximizeWindowCommand+��5/      �� $ 	ucMenuBar��.��52        $ WindowMaximize=�62   
�������53   2   54      �� 55      #��[ 58      
���\\����56      �� *CloseWindowCommand+��57      �� $ 	ucMenuBar��.��5:        $ WindowClose=�62   
�������5;   2   5<      5=      �� 5>      $
��Left=�6   $ 
Horizontal=�63   .+�5?      �� $��../Images/LogoDeCo.png��6   $��30��6<   $	��100��6H   $
��Left=�5@      $
��10,0q�67   5A      5B      5C      5D      5E      