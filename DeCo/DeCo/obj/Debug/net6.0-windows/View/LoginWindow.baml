   M S B A M L   `   `   `  ����?  ;DeCo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:DeCo.UserControlUIDeCo.UserControlUI  5   	   %clr-namespace:DeCo.View	DeCo.View  5   	       DeCo.View.LoginWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /uc clr-namespace:DeCo.UserControlUI     5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   #localclr-namespace:DeCo.View  5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5
      -   5        $��loginWindow��5   	      MaterialDesignBody#���   5      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions    TextFormattingMode$  Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#���  5       � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	     LoginVM � DataContext# [ 5   	    � Title$ LoginWindow��5   	   $	��400��6   $	��400��6*   .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��* �6   6)   .+�5   
   � $��0��6   .+�5           DeCo.UserControlUI.MenuBar  5!      � 5"   
   $��1��6   ���#E3E7FF.+�5#      �� $��Log in��6   $��35(�5$      $
��Bold"�5%      ���#574AED$��30,30q�5'      �� 5)       �� Orientation$ Vertical=�6   $��Center=�60   $	��167��6K   	 +� FocusedElement	 5*      �� 
 �� ElementName$
 UsernameBox��.+�5-      �� $��5q�6   $ 
Horizontal=�6(   $��Center=�6A   .+�5.      �� $��1q�6   
���   �
����� �._�50      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Account=�62   $��50��51   .   $��30��52   .   $��Center=�53   .   $��Center=�54   .   
�������55   2   57      �� 58      $��1q�6   
���   �._�5:      �� -   5         ��Name$ UsernameBox��5;      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$    Username��5<      $��5q�5=      $��Champagne & Limousines)�5>      $��18(�5?       % !MaterialDesignFloatingHintTextBox#��[ 5@      $	��280��5A   3   ��6   �� *
Username+63   5B      5C      �� 5E      $��5q�6   $ 
Horizontal=�6(   $��Center=�6A   .+�5F      �� $��1q�6   
���   �
����� �._�5G        $
 Lock=�62   $��50��5H   .   $��30��5I   .   $��Center=�5J   .   $��Center=�5K   .   
�������5L   .   5M      �� 5N      $��1q�6   
���   �._�5O      G� -   5        $��FloatingPasswordBox��5P      $��5q�5Q      $    Password��5R      $��Champagne & Limousines)�5S      $��18(�5T       ) %MaterialDesignFloatingHintPasswordBox#��[ 5U      $	��280��5V      .  (System.Windows.Interactivity.Interaction   Triggers 5W      /  )System.Windows.Interactivity.EventTrigger  5X   "      	EventName$ PasswordChanged��61   .  (System.Windows.Interactivity.TriggerBase   Actions. 5Y   &   6  0System.Windows.Interactivity.InvokeCommandAction     Command 6<   �� *PasswordChangedCommand+   CommandParameter 6g   �� $
 FloatingPasswordBox��5Z   #   5\      5]      5^      �� 5_      $
��30,5q�6   $��15(�6*    ! MaterialDesignHelperTextBlock#��[ 5a      
���\\����5`      �� *
loginMsg+5b      5c      �� 5d      $��100,280,100,40q�6   $��45��6-   $	��200��69   ���#6272FF$��Log in��5f      $��20(�5g      $��Normal"�6#   
�������$��2q�6K   ��5h      �� *LoginCommand+��66   �� $
 loginWindow��5j      5k      5l      5n      