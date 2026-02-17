// ===============================================================================
// FileNameChenger
//
// Namespace : FileNameChenger
// ClassName : CFileCopy.cs
//
// ===============================================================================
// Release history
// VERSION	AUTHER             DATE       DESCRIPTION
//   1.0	Masami Shiohara  2009.02.14   新規
//
// ===============================================================================
// Copyright (C) 2009 
// All rights reserved.
// ===============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace FileNameChengerCore
{

    /// *************************************************************
    /// <summary>処理ステータス</summary>
    /// *------------------------------------------------------------
    /// 備考：<remarks></remarks>
    /// *************************************************************
    public enum EStateFlg
    {
        /// <summary>初期</summary>
        Non,
        /// <summary>処理中</summary>
        Run,
        /// <summary>処理終了</summary>
        End,
        /// <summary>中断</summary>
        Cancel
    }

    /// *************************************************************
	/// <summary>ファイルコピークラス</summary>
	/// *------------------------------------------------------------
	/// 備考：<remarks>ファイルコピーを行うクラスソースファイル</remarks>
	/// *************************************************************
    public class CFileCopy 
    {
        EStateFlg eStateFlg = EStateFlg.Non;
      	/// *============================================================
		/// <summary>変換中ﾌﾗｸﾞ(ﾌﾟﾛﾊﾟﾃｨ)</summary>
		/// * -----------------------------------------------------------
		/// 備考	：<remarks>処理中断に利用します。</remarks>
		/// *============================================================
        public EStateFlg StateFlg
        {
            get
            {
                return eStateFlg; 
            }
        }

        bool bSubDir = false;
        /// *============================================================
        /// <summary>サブディレクトリまで見るかどうかﾌﾗｸﾞ(ﾌﾟﾛﾊﾟﾃｨ)</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public bool IsSubDir
        {
            get
            {
                return bSubDir;
            }
        }

        string strSourcePath = "";
        /// *============================================================
        /// <summary>コピー元(ﾌﾟﾛﾊﾟﾃｨ)</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public string SourcePath
        {
            get
            {
                return strSourcePath;
            }
        }

        string strTargetPath = "";
        /// *============================================================
        /// <summary>コピー先(ﾌﾟﾛﾊﾟﾃｨ)</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public string TargetPath
        {
            get
            {
                return strTargetPath;
            }
        }

        //TODO:ファイルタイプは選択できるようにしないとだめ！！
        //string strSerchPattern = "*";
        string strSerchPattern = CConfig.DefultSerchPattern;
        public string SerchPattern
        {
            get
            {
                return strSerchPattern;
            }
        }

        string strFileNamePattern = CConfig.DefultFileNamePattern;
        public string FileNamePattern
        {
            get
            {
                return strFileNamePattern;
            }
        }


        List<string> files = null;
        /// *============================================================
        /// <summary>処理対象件数(ﾌﾟﾛﾊﾟﾃｨ)</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public int Count
        {
            get
            {
                if (files == null)
                {
                    return 0;
                }
                else
                {
                    return files.Count;
                }
            }
        }

        bool bFreeSpace = true;
        /// *============================================================
        /// <summary>ディスク容量の☑(ﾌﾟﾛﾊﾟﾃｨ)</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public bool FreeSpace
        {
            get
            {
                return bFreeSpace;
            }
        }

      	/// *============================================================
		/// <summary>ｺﾝｽﾄﾗｸﾀ</summary>
		/// * -----------------------------------------------------------
        /// 引数	：<param name="in_SourcePath">コピー元</param>
        ///           <param name="in_TargetPath">コピー先</param>
        ///           <param name="in_SearchPattern">検索パターン</param>
        ///           <param name="in_FileNamePattern">作成ファイル名パターン</param>
        ///           <param name="in_IsSubDir">サブディレクトリまで見るかフラグ</param>
        /// 備考	：<remarks>このｸﾗｽのｺﾝｽﾄﾗｸﾀです。</remarks>
		/// *============================================================
        public CFileCopy(string in_SourcePath, string in_TargetPath,
            string in_SearchPattern, string in_FileNamePattern, bool in_IsSubDir)
        {
            //TODO:作成先のファイル名は日付を基本するが、
            //     フォーマットの設定も可能にしたいです。
            strSourcePath = in_SourcePath;
            strTargetPath = in_TargetPath;
            strSerchPattern = in_SearchPattern;
            strFileNamePattern = in_FileNamePattern;
            bSubDir = in_IsSubDir;

            //対象件数取得
            files = getFiles(strSourcePath, strSerchPattern, bSubDir);

            bFreeSpace = checkFreeSpace();

         }

        /// *============================================================
        /// <summary>ｺﾝｽﾄﾗｸﾀ</summary>
        /// * -----------------------------------------------------------
        /// 引数	：<param name="in_SourcePath">コピー元</param>
        ///           <param name="in_TargetPath">コピー先</param>
        /// 備考	：<remarks>このｸﾗｽのｺﾝｽﾄﾗｸﾀです。</remarks>
        /// *============================================================
        public CFileCopy(string in_SourcePath, string in_TargetPath)
            : this(in_SourcePath, in_TargetPath, CConfig.DefultSerchPattern, CConfig.DefultFileNamePattern, false)
        {
        }

              	/// *============================================================
		/// <summary>ｺﾝｽﾄﾗｸﾀ</summary>
		/// * -----------------------------------------------------------
        /// 引数	：<param name="in_SourcePath">コピー元</param>
        ///           <param name="in_TargetPath">コピー先</param>
        ///           <param name="in_SearchPattern">検索パターン</param>
        ///           <param name="in_FileNamePattern">作成ファイル名パターン</param>
        /// 備考	：<remarks>このｸﾗｽのｺﾝｽﾄﾗｸﾀです。</remarks>
		/// *============================================================
        public CFileCopy(string in_SourcePath, string in_TargetPath,
            string in_SearchPattern, string in_FileNamePattern)
            : this(in_SourcePath, in_TargetPath, in_SearchPattern, in_FileNamePattern, false)
        {
        }

        private bool checkFreeSpace()
        {
            //TODO:ファイル名をPATHに入れられた場合の処理を考え直そう。
            if (files == null)
            {
                return true; 
            }


            //ドライブスペースの検証
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (strTargetPath.IndexOf(d.Name) >= 0)
                {
                    long free = d.TotalFreeSpace;
                    foreach (string s in files)
                    {
                        FileInfo f = new FileInfo(s);
                        free = free - f.Length;
                        if (free <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// *============================================================
        /// <summary>ファイル一覧取得処理</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        private List<string> getFiles(string in_SourcePath, string in_SearchPattern, bool in_IsSubDir)
        {
            //TODO:一覧取得するとディレクトリ毎にスレッドという野望が達成できないが、
            //　　 HDへの負荷は少ないと思われる。
            //TODO:履歴管理するとなると、そのたびに一覧取得する処理が必要になるのでは？

			//取得時にソートできたらしておいたほうが、作成日時での順番も守れるのでは？
            if (!Directory.Exists(in_SourcePath))
            {
                return null;
            }
            
            SearchOption so = SearchOption.AllDirectories;
            if (!in_IsSubDir)
            {
                so = SearchOption.TopDirectoryOnly;
            }

            string[] searchPatterns = in_SearchPattern.Split(';');
            List<string> fileList = new List<string>();
            foreach (string searchPattern in searchPatterns)
            {
                 fileList.AddRange(Directory.GetFiles(in_SourcePath, searchPattern, so));
            }
            
            return fileList;
        }


        /// *============================================================
        /// <summary>キャンセル発生処理</summary>
        /// * -----------------------------------------------------------
        /// 備考	：<remarks></remarks>
        /// *============================================================
        public void Cancel()
        {
            eStateFlg = EStateFlg.Cancel;
        }

   		/// *============================================================
		/// <summary>実行初期処理</summary>
		/// * -----------------------------------------------------------
		/// 備考	：<remarks></remarks>
		/// *============================================================
        public void Run()
        {
            //スレッドプール
            //ThreadPool 

            ThreadStart threadDelegate = new ThreadStart(DoWork);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();
        }



    	/// *============================================================
		/// <summary>コピーメイン処理</summary>
		/// * -----------------------------------------------------------
		/// 備考	：<remarks>スレッドで処理を行う</remarks>
		/// *============================================================
        private void DoWork()
        {
            //TODO:元のファイルを削除するか選択できるように対応予定
            //TODO:サブディレクトリに対応予定
            if (!System.IO.Directory.Exists(strTargetPath))
            {
                System.IO.Directory.CreateDirectory(strTargetPath);
            }

            try
            {

                foreach (string strSourcePath in files)
                {
                    if (eStateFlg == EStateFlg.Cancel)
                    {
                        break;
                    }

                    FileInfo fileInfoSource = new FileInfo(strSourcePath);

                    //string fileName = fileInfoSource.Name;
                    string fileExtension = fileInfoSource.Extension;

                    //TODO:更新日時、作成日時、アクセス日時の選択ができるように対応予定
                    //string fileName = fileInfoSource.CreationTime.ToString("yyyyMMdd-HHmmss-ffff");
                    string fileName = fileInfoSource.LastWriteTime.ToString(strFileNamePattern);

                    string fullName = System.IO.Path.Combine(strTargetPath, fileName + fileExtension);

                    //TODO:同一時刻で異なる場合もあるので最終的にはbit単位でチェックしてコピーしたい
                    bool exists = File.Exists(fullName);
                    if (!exists)
                    {
                        try
                        {
                            fileInfoSource.CopyTo(fullName);
                            OnDoWorkingEvent(new DoWorkingEventArgs(fileInfoSource.FullName, fullName, exists));
                        }
                        catch (Exception e)
                        {
                            //ディスク許容量エラーなど
                            OnDoWorkingEvent(new DoWorkingEventArgs(fileInfoSource.FullName, fullName, exists, e.ToString()));
                            //処理終了
                            return;
                        }
                    }
                    else
                    {
                        //TODO:ある場合どうしましょう？
                        OnDoWorkingEvent(new DoWorkingEventArgs(fileInfoSource.FullName, fullName, exists));
                    }
                }
            }
            finally
            {
                OnDoWorkEndEvent(new EventArgs());
            }
        }

        public event EventHandler<DoWorkingEventArgs> DoWorkingEvent;

        protected virtual void OnDoWorkingEvent(DoWorkingEventArgs e)
        {
            EventHandler<DoWorkingEventArgs> handler = DoWorkingEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<EventArgs> DoWorkEndEvent;
        protected virtual void OnDoWorkEndEvent(EventArgs e)
        {
            EventHandler<EventArgs> handler = DoWorkEndEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }
 
        
    }

    //イベントのデザイン
    //http://msdn.microsoft.com/ja-jp/library/ms229011.aspx
    public class DoWorkingEventArgs : EventArgs
    {
        string strFileNameFrom = "";
        public string FileNameFrom
        {
            get
            {
                return strFileNameFrom;
            }
        }

        string strFileNameTo = "";
        public string FileNameTo
        {
            get
            {
                return strFileNameTo;
            }
        }

        bool bExists = false;
        public bool Exists
        {
            get
            {
                return bExists;
            }
        }

        string strErr = "";
        public string Err
        {
            get
            {
                return strErr;
            }
        }

        public DoWorkingEventArgs(string in_FileNameFrom, string in_FileNameTo, bool in_Exists, string in_Err)
        {
            strFileNameFrom = in_FileNameFrom;
            strFileNameTo = in_FileNameTo;
            bExists = in_Exists;
            strErr = in_Err;
        }

        public DoWorkingEventArgs(string in_FileNameFrom, string in_FileNameTo, bool in_Exists)
            : this(in_FileNameFrom, in_FileNameTo, in_Exists, "")
        {
        }

    }

    //public delegate void EventDoWorking(object sender, EventArgesDoWorking e);
}
