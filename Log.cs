using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// �T�E���h�V�X�e����p�̃��M���O�N���X<para></para>
/// - �G�f�B�^��̃��O�ɉ����A���O�t�@�C�����c��<para></para>
///   �G�f�B�^�ł̃p�X�FApplication.dataPath, "../Logs"<para></para>
///   �r���h�łł̃p�X�FApplication.persistentDataPath<para></para>
/// - �J�e�S�����ɂ͌����Ƃ��ČĂяo�����̃X�N���v�g��(�g���q�Ȃ�)���g�p<para></para>
/// - SoundSystem�n�̓����݌v�ɂ�����1�t�@�C�� = 1�N���X�\���ł��邱�Ƃ�O��Ƃ��Ă���<para></para>
/// - �����N���X��1�t�@�C���ɒ�`�����ꍇ�A���O�J�e�S�����B���ɂȂ�\��������
/// (�K�v�ł���΃��O�Ăяo�����ɃJ�e�S���𖾎��I�Ɏw�肵�Ώ��\)
/// </summary>
internal static class Log
{
    private static StreamWriter fileWriter;
    private static readonly object locker = new();
    private static bool isInitialized = false;

    public enum LogLevel
    {
        Info,
        Warn,
        Error
    }

    public static void Initialize(string fileName = "SoundLog.txt")
    {
        if(isInitialized) return;

        string logDirectory;
#if UNITY_EDITOR
        logDirectory = Path.Combine(Application.dataPath, "../Logs");
#else
        logDirectory = Application.persistentDataPath;
#endif
        Directory.CreateDirectory(logDirectory);
        string logPath = Path.Combine(logDirectory, fileName);

        try
        {
            fileWriter = new(logPath, append: true);
            fileWriter.AutoFlush = true;
            isInitialized = true;
            Safe($"Initialize����: {logPath}");
        }
        catch (Exception e)
        {
            Error($"Initialize���s,{e.Message}");
        }
    }

    /// <param name="category">�����͂Ȃ�Ăяo�����̃N���X��������</param>
    public static void Safe(string message, [CallerFilePath] string category = "")
    {
        category = Path.GetFileNameWithoutExtension(category);
        Output(LogLevel.Info, category, message);
    }

    /// <param name="category">�����͂Ȃ�Ăяo�����̃N���X��������</param>
    public static void Warn(string message, [CallerFilePath] string category = "")
    {
        category = Path.GetFileNameWithoutExtension(category);
        Output(LogLevel.Warn, category, message);
    }

    /// <param name="category">�����͂Ȃ�Ăяo�����̃N���X��������</param>
    public static void Error(string message, [CallerFilePath] string category = "")
    {
        category = Path.GetFileNameWithoutExtension(category);
        Output(LogLevel.Error, category, message);
    }

    private static void Output(LogLevel level, string category, string message)
    {
        if (fileWriter == null)
        {
            return;
        }

        string timestamp   = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string fullMessage = $"[{timestamp}] [{level}] [{category}] {message}";

#if UNITY_EDITOR
        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(fullMessage);
                break;

            case LogLevel.Warn:
                Debug.LogWarning(fullMessage);
                break;

            case LogLevel.Error:
                Debug.LogError(fullMessage);
                break;
        }
#endif

        //�Ăяo����(��FSEManager)�ɔ񓯊������������̂ŁA�����̗\�h
        lock (locker)
        {
            fileWriter.WriteLine(fullMessage);
        }
    }

    public static void Close()
    {
        fileWriter?.Flush();
        fileWriter?.Close();
        fileWriter = null;
    }
}