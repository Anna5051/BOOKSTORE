   M S B A M L   `   `   `  ����  WpfApp1     WpfApp1.Pages.Registration   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  Registration��5       N� 
Background �#F2F4F8 � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name   RoundedButtonTemplate	 ��Name
 �� Triggers P� Property P� Value �� 
TargetName3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Color   Opacity �� ScaleX �� ScaleY  GreenRoundedButtonStyle �� Triggers  OrangeRoundedButtonStyle��%�	  '��       'G� �    &	  �    &	 �    7   &	 �    7   �� 5
   
   " ��6   .�5      ��  � ��$ 0,4,0,10q�6'   �� 5       � ��$ 8q�6(   �� 5       � �� �#F7F9FC6+   �� 5       � �� �#DCE3EC6,   �� 5       � ��$ 1q�60   �� 5       � ��$ 34��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 6��6!   #��z��5   !   #��z��5   !   #��z��5   !   ._�5      �� $ PART_ContentHost��6+   5      5      5      5      �� 5    
   " G�6   .�5!      ��  � ��$ 0,4,0,10q�6'   �� 5"       � ��$ 8q�6(   �� 5#       � �� �#F7F9FC6+   �� 5$       � �� �#DCE3EC6,   �� 5%       � ��$ 1q�60   �� 5&       � ��$ 34��6'   �� 5'       � �� 5(      �� 5)      " G�6&   .R�5*      �� $ 6��6!   #��z��5+   !   #��z��5,   !   #��z��5-   !   ._�5.      �� $ PART_ContentHost��6+   5/      50      52      53      �� 56   
   " ��68   .R�57      �� $	 border��6   #��z��58      $ 8��59      #��z��5:      ._�5;      �� $��Center=�6#   $��Center=�6@   5<      
 5=      P� 5>       �@J�IsMouseOver . . �5?      �� $ border��6    �@J�Effect 5@        5A      $ 6Q�6/   $ 2Q�6>   $
 Gray��6N   $	 0.4Q�6[   5C      5D      P� 5E       � �� . . �5F      �� $ border��6    � z� 5G      �� 5H      $
 0.98Q�6-   $
 0.98Q�6;   5J      5K      5M      �� 5P   
   " ��60   .�5Q      ��  � ��$ 36��6'   �� 5R       � ��$	 120��6&   �� 5S       � ��
 �����6+   �� 5T       � ��$ SemiBold"�6+   �� 5U       � ��$ 0q�60   �� 5V       �@�Cursor$
 Hand��6'   �� 5W       � �� �#1976D26+   �� 5X       � ��8   6)    5Y      P� 5Z       �@J�IsMouseOver . . �5[      ��  � �� �#60B0FF63   5\      5_      �� 5b   
   " ��61   .�5c      ��  � ��$ 36��6'   �� 5d       � ��$	 120��6&   �� 5e       � ��
 �����6+   �� 5f       � ��$ SemiBold"�6+   �� 5g       � ��$ 0q�60   �� 5h       �@�Cursor$
 Hand��6'   �� 5i       � �� �#31B2B76+   �� 5j       � ��8   6)    5k      P� 5l       �@J�IsMouseOver . . �5m      ��  � �� �#A8DADC63   5n      P� 5o       � �� . . �5p      ��  � �� �#A8DADC63   5q      5s      5u      .��5w      � .+�5x   
   �� 
�������$ 16��5y       �� Padding$ 30q�5z      $	��450��5{      ���#DCE3EC$��1q�5}      $��Center=�5~      $��Center=�5      ._�5�      �� .+�5�      �� $��Регистрация��6   $��26(�5�      $
��Bold"�5�      ���#333$��Center=�5�      $��0,0,0,20q�5�      �� 5�      $��Имя��6   $��0,4,0,0q�6'   $
��Bold"�68   ���#5556J   �� 5�      -   5         ��Name$ nameTb��5�      �� 5�      $��
Логин��6   $��0,4,0,0q�6)   $
��Bold"�6:   ���#5556L   �� 5�      -   5        $ loginTb��5�      �� 5�      $��Пароль��6   $��0,4,0,0q�6*   $
��Bold"�6;   ���#5556M   G� 5�      -   5         G�Name$ 
passwordTb��5�      �� 5�      $%��Повторите пароль��6   $��0,4,0,0q�64   $
��Bold"�6E   ���#5556W   G� 5�      -   5        $ repeatPasswordTb��5�      �� 5�      $��Email��6   $��0,4,0,0q�6)   $
��Bold"�6:   ���#5556L   �� 5�      -   5        $ emailTb��5�      �� 5�      $��Дата рождения��6   $��0,4,0,0q�61   $
��Bold"�6B   ���#5556T   (  "System.Windows.Controls.DatePicker  5�      -   5          Name$ birthDateTb��5�      $��34��5�      ���#F7F9FC���#DCE3EC$��1q�5�      $��0,4,0,10q�5�      �� 5�      $��Телефон��6   $��0,4,0,0q�6+   $
��Bold"�6<   ���#5556N   �� 5�      -   5        $ phoneTb��5�      �� 5�       �� Orientation$ 
Horizontal=�6   $
��Left=�66   $��0,10,0,0q�6Q   .+�5�      �� -   5         ��Name$ RegistrationButton��5�      $��Регистрация��5�      #��[ 5�      $��0,0,10,0q�5�      �� 5�      -	   5        $ 
BackButton��5�      $��
Назад��5�      #��[ 5�      5�      5�      5�      5�      5�      5�      