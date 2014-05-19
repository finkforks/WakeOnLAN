'    WakeOnLAN - Wake On LAN
'    Copyright (C) 2004-2014 Aquila Technology, LLC. <webmaster@aquilatech.com>
'
'    This file is part of WakeOnLAN.
'
'    WakeOnLAN is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    WakeOnLAN is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with WakeOnLAN.  If not, see <http://www.gnu.org/licenses/>.

Imports System.Linq
Imports System.IO

Module Globals
    Private Declare Function FormatMessageA Lib "kernel32" (ByVal flags As Integer, ByRef source As Object, ByVal messageID As Integer, ByVal languageID As Integer, ByVal buffer As String, ByVal size As Integer, ByRef arguments As Integer) As Integer
    Public Declare Function InitiateSystemShutdown Lib "advapi32.dll" Alias "InitiateSystemShutdownA" (ByVal lpMachineName As String, ByVal lpMessage As String, ByVal dwTimeout As Integer, ByVal bForceAppsClosed As Integer, ByVal bRebootAfterShutdown As Integer) As Integer
    Public Declare Function AbortSystemShutdown Lib "advapi32.dll" Alias "AbortSystemShutdownA" (ByVal lpMachineName As String) As Integer
    Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Long) As IntPtr
    Public Declare Function ShowWindow Lib "user32" (ByVal hWnd As IntPtr, ByVal nCmdShow As Int32) As Boolean
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr

    Public ShutdownMode As Boolean      'true if in shutdown mode, false if in abort mode
    Public meItem As ListViewItem
    Public splashPtr As IntPtr

    Public Function FormatMessage(ByVal [error] As Integer) As String
        Const FORMAT_MESSAGE_FROM_SYSTEM As Short = &H1000
        Const LANG_NEUTRAL As Short = &H0
        Dim buffer As String = Space(999)
        FormatMessageA(FORMAT_MESSAGE_FROM_SYSTEM, 0, [error], LANG_NEUTRAL, buffer, 999, 0)
        buffer = Replace(Replace(buffer, Chr(13), ""), Chr(10), "")
        Return buffer.Substring(0, buffer.IndexOf(Chr(0)))
    End Function

    Public Sub ShowHelp(parent As Control, url As String)
        Try
#If DEBUG Then
            Help.ShowHelp(parent, "file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\..\help\" + url))
#Else
            Help.ShowHelp(parent, "file:///" + AppDomain.CurrentDomain.BaseDirectory + "help\" + url)
#End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Help Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Public Function SaveListViewState(ByVal listview As ListView) As String

        Return listview.Columns.Cast(Of ColumnHeader)().Aggregate("", Function(current, c) current & (c.Width & " "))

    End Function

    Public Sub GetListViewState(ByVal listview As ListView, ByVal State As String)
        Dim s() As String
        Dim i As Int16

        s = Split(State)
        If (UBound(s) <> listview.Columns.Count) Then Exit Sub

        For i = 0 To UBound(s) - 1
            listview.Columns(i).Width = Int(s(i))
        Next
    End Sub

End Module
