   M S B A M L   `   `   `  ����E  Akonfirence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:konfirence.Pageskonfirence.Pages  5           konfirence.Pages.StartPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:konfirence.Pages     5        N� Title$  	StartPage��5	       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $��70 �6   �� 5      �� 5      $��70 �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $
��Left=�6;   $��Center=�5      $��10q�60   .+�5      �� -   5         ��Name$ DirectionComboBox��5      $	��200��62   $��10q�6>    �� DisplayMemberPath$
 Name��5   A   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ 
DatePicker��5      $	��200��6-      SelectedDateFormat$ Short=�5      5      �� 5      -   5         ��Name$ Listing��5   $   $��1��6   $��10q�65   ��5      �� 5      .R�5       � 	 � ColumnDefinitions	 5!      �� 5"      61   �� 5#      61   .+�5%      �� $��0��6    $	��100��6M   $	��100��6Y   $��5q�6f   ��60   �� *	ImagePath+6f   �� 5'      $��1��6%   $ Vertical=�65   $��5q�6L   .+�5(      �� $
��Bold"�6@   $��14(�6R   ��6$   �� *
EventTopic+6R   �� 5*      $��12(�6D   $��0,5,0,0q�6R   ��6$   �� *EventDirection+6R   �� 5,      $��12(�6Y   ��6$   �� *Date+
 �� StringFormat$
 {0:yyyy-MM-dd}��6Y   5.      5/      51      53      �� 55   
   -   5         ��Name$ AutorizeButton��55      $��2��6F   $��Авторизация��6S   $��Center=�56      $��Center=�6.   $��50��6I   $	��200��6U   57      58      