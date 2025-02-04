﻿Public Class Menu
    Private m_cancelled As Boolean
    Private m_selected As Boolean
    Private dpiX, dpiY As Single

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Using g As Graphics = Me.CreateGraphics()
            dpiX = g.DpiX
            dpiY = g.DpiY
        End Using
    End Sub

    Public WriteOnly Property Options() As IDictionary(Of String, String)
        Set(value As IDictionary(Of String, String))
            For Each key As String In value.Keys
                lstOptions.Items.Add(key, value(key), "")
            Next
            If lstOptions.Items.Count > 0 Then
                lstOptions.Items(0).Selected = True
            End If
        End Set
    End Property

    Public WriteOnly Property Caption() As String
        Set(value As String)
            lblCaption.Text = value
            DoResize()
        End Set
    End Property

    Public WriteOnly Property AllowCancel() As Boolean
        Set(value As Boolean)
            cmdCancel.Visible = value
            Me.ControlBox = value
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As String
        Get
            If m_cancelled Then Return Nothing
            If lstOptions.SelectedItems.Count = 0 Then Return Nothing
            Return lstOptions.SelectedItems(0).Name
        End Get
    End Property

    Private Sub cmdSelect_Click(sender As System.Object, e As System.EventArgs) Handles cmdSelect.Click
        ItemSelected()
    End Sub

    Private Sub ItemSelected()
        m_selected = True
        Me.Close()
    End Sub

    Private Sub Menu_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        m_cancelled = Not m_selected
    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click
        m_cancelled = True
        Me.Close()
    End Sub

    Private Sub lstOptions_DoubleClick(sender As Object, e As System.EventArgs) Handles lstOptions.DoubleClick
        ItemSelected()
    End Sub

    Private Sub lstOptions_Resize(sender As Object, e As System.EventArgs) Handles lstOptions.Resize
        lstOptions.Columns(0).Width = lstOptions.Width - CInt(40 * dpiX / 96)
    End Sub

    Private Sub Menu_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        DoResize()
    End Sub

    Private Sub DoResize()
        lblCaption.MaximumSize = New Size(Me.Width - CInt(24 * dpiX / 96), lblCaption.MaximumSize.Height)
        lstOptions.Top = lblCaption.Top + lblCaption.Height + CInt(8 * dpiY / 96)
        lstOptions.Height = Me.Height - (lstOptions.Top + CInt(76 * dpiY / 96))
    End Sub

End Class