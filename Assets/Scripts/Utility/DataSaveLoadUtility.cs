using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using SimpleSQL;
using UnityEngine;

namespace QFramework.Example
{
    public class DataSaveLoadUtility : IUtility
    {
        /// <summary>
        /// 学生数据
        /// 跳转运行时类型<see cref="StudentData"/>
        /// </summary>
        [Serializable]
        public class StudentDB
        {
            [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
            public int StudentId { get; set; }

            public string name { get; set; }

            public int grade { get; set; }

            public string startTime { get; set; }

            public string endTime { get; set; }

            public string location { get; set; }

            public string weekday { get; set; }

            public string classNumber { get; set; }

            //价格
            public float price { get; set; }

            public StudentDB()
            {

            }

            public StudentDB(StudentData studentData)
            {
                this.name = studentData.name;
                this.grade = studentData.grade;
                this.startTime = studentData.startTime.ToString("yyyy-MM-dd");
                this.endTime = studentData.endTime.ToString("yyyy-MM-dd");
                this.location = studentData.location;
                this.weekday = string.Join(",", studentData.weekday);
                this.classNumber = string.Join(",", studentData.classNumber);
                this.price = studentData.price;
            }
        }

        [Serializable]
        public class TimetableItemDB
        {
            [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
            public int id { get; set; }

            public int studentId { get; set; }

            public string time { get; set; }

            public int classNumber { get; set; }

            public TimetableItemDB()
            {

            }

            public TimetableItemDB(int studentId, TimetableItemData timetableItemData)
            {
                this.studentId = studentId;
                this.time = timetableItemData.time.ToString("yyyy-MM-dd");
                this.classNumber = timetableItemData.classNumber;
            }
        }
        public class sqlite_master
        {
            public string type { get; set; }
            public string name { get; set; }
            public string tbl_name { get; set; }
            public int rootpage { get; set; }
            public string sql { get; set; }
        }
        SimpleSQLManager mSqlManager;

        public void SaveData(TimetableItemData timetableItemData)
        {
            CheckHaveTable();
            // 插入数据

            StudentDB studentDB = new StudentDB(timetableItemData.studentData);
            long studentId = GetExistingTimetableItemId(studentDB);
            if (studentId >= 0)
            {
                Debug.Log("学生数据已存在，更新数据");
            }
            else
            {
                mSqlManager.Insert(studentDB, out studentId);
                Debug.Log("插入学生数据成功，studentId：" + studentId);
            }

            TimetableItemDB timetableItemDB = new TimetableItemDB((int) studentId, timetableItemData);
            mSqlManager.Insert(timetableItemDB);
        }

        //获取已存在的学生数据
        private int GetExistingTimetableItemId(StudentDB studentDB)
        {
            bool recordExists = false;
            string query =
                "SELECT studentId FROM StudentDB WHERE name = ? AND grade = ? AND startTime = ? AND endTime = ? AND location = ? AND weekday = ? AND classNumber = ? AND price = ?";
            StudentDB result = mSqlManager.QueryFirstRecord<StudentDB>(out recordExists, query,
                studentDB.name,
                studentDB.grade,
                studentDB.startTime,
                studentDB.endTime,
                studentDB.location,
                studentDB.weekday,
                studentDB.classNumber,
                studentDB.price);
            if (recordExists)
            {
                return result.StudentId;
            }

            return -1;
        }

        public List<TimetableItemData> LoadData()
        {
            if (CheckHaveTable())
            {
                var timetableItemDBs =
                    new List<TimetableItemDB>(from tidb in mSqlManager.Table<TimetableItemDB>() select tidb);
                var studentDBs = new List<StudentDB>(from sdb in mSqlManager.Table<StudentDB>() select sdb);
                List<TimetableItemData> timetableItemDataList = new List<TimetableItemData>();
                foreach (var item in timetableItemDBs)
                {
                    var studentDB = studentDBs.FirstOrDefault(s => s.StudentId == item.studentId);
                    if (studentDB == null)
                    {
                        Debug.LogError($"item.studentId:{item.studentId} 学生数据不存在");
                        continue;
                    }

                    var studentData = new StudentData();
                    studentData.name = studentDB.name;
                    studentData.grade = studentDB.grade;
                    studentData.startTime = studentDB.startTime.ToDateTime();
                    studentData.endTime = studentDB.endTime.ToDateTime();
                    studentData.location = studentDB.location;
                    studentData.weekday = new List<int>();
                    studentData.classNumber = new List<int>();
                    for (int i = 0; i < studentDB.weekday.Split(',').Length; i++)
                    {
                        int weekday = int.Parse(studentDB.weekday.Split(',')[i]);
                        studentData.weekday.Add(weekday);
                    }

                    for (int i = 0; i < studentDB.classNumber.Split(',').Length; i++)
                    {
                        int classNumber = int.Parse(studentDB.classNumber.Split(',')[i]);
                        studentData.classNumber.Add(classNumber);
                    }

                    TimetableItemData timetableItemData = new TimetableItemData();
                    timetableItemData.studentData = studentData;
                    timetableItemData.time = item.time.ToDateTime();
                    timetableItemData.classNumber = item.classNumber;
                    timetableItemDataList.Add(timetableItemData);
                }

                return timetableItemDataList;
            }

            Debug.LogError("数据库没有表，请检查");
            return new List<TimetableItemData>();
        }

        public void ClearData()
        {
            mSqlManager.Execute("Delete from TimetableItemDB");
            mSqlManager.Execute("Delete from StudentDB");
        }
        public bool CheckHaveTable()
        {
            if (mSqlManager == null)
            {
                mSqlManager = GameObject.FindFirstObjectByType<SimpleSQLManager>();
            }

            // 构建查询语句
            string query = "SELECT * FROM sqlite_master WHERE name=?";

            // 执行查询
            var tableNames = mSqlManager.Query<sqlite_master>(query, "TimetableItemDB");
            // 执行查询
            var tableNames1 = mSqlManager.Query<sqlite_master>(query, "StudentDB");
            Debug.Log("TimetableItemDB "+ tableNames.Count);
            Debug.Log("StudentDB " + tableNames1.Count);

            if (tableNames.Count <= 0 || tableNames1.Count <= 0)
            {
                // 创建表
                mSqlManager.CreateTable<TimetableItemDB>();
                mSqlManager.CreateTable<StudentDB>();
                return false;
            }

            return true;
        }
    }
}