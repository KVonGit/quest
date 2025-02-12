﻿Public Class BrowseFilter
    Public Event CategoryChanged(category As String)

    Public Sub Populate(cats As String())
        lstCategories.Items.Clear()
        For Each cat As String In cats
            lstCategories.Items.Add(cat)
        Next
        If lstCategories.Items.Count > 0 Then
            lstCategories.SelectedIndex = 0
            RaiseEvent CategoryChanged(lstCategories.Text)
        End If
    End Sub

    Private Sub lstCategories_SelectionChanged(sender As System.Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        RaiseEvent CategoryChanged(DirectCast(DirectCast(sender, System.Windows.Controls.ComboBox).SelectedValue, String))
    End Sub

    Public ReadOnly Property Category As String
        Get
            Return DirectCast(lstCategories.SelectedValue, String)
        End Get
    End Property
End Class
