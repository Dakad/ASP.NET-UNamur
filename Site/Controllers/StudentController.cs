﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UNamur.DAL;
using UNamur.Models;

namespace UNamur.Controllers
{
	public class StudentController : Controller
	{
		private SchoolContext db = new SchoolContext();

		// GET: Student
		public ActionResult Index(string sortOrder, string searchStr)
		{
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DateSortParm = (sortOrder == "Date" ? "date_desc" : "Date");
			// No Query is sent to DB. The query is not executed until 
			// you convert the IQueryable object into a collection via .ToList()
			IQueryable<Student> students = from s in db.Students
										   select s;
			if (!String.IsNullOrEmpty(searchStr))
			{
				students = students.Where(s =>	s.LastName.ToUpper().Contains(searchStr.ToUpper())
												||
												s.FirstMidName.ToUpper().Contains(searchStr.ToUpper())
										);

			}
			switch (sortOrder)
			{
				case "name_desc":
					students = students.OrderByDescending(s => s.LastName);
					break;
				case "Date":
					students = students.OrderBy(s => s.EnrollmentDate);
					break;
				case "date_desc":
					students = students.OrderByDescending(s => s.EnrollmentDate);
					break;
				default:
					students = students.OrderBy(s => s.LastName);
					break;
			}
			return View(students.ToList());
		}

		// GET: Student/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Student student = db.Students.Find(id);
			if (student == null)
			{
				return HttpNotFound();
			}
			return View(student);
		}

		// GET: Student/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Student/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken] // Helps preventcross-site request forgeryattacks
		public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate", Exclude = "ID")] Student student)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Students.Add(student);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (DataException /* ex */)
			{
				// Log the error (uncomment dex variable name and add a linehere to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}

			return View(student);
		}

		// GET: Student/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Student student = db.Students.Find(id);
			if (student == null)
			{
				return HttpNotFound();
			}
			return View(student);
		}

		// POST: Student/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry(student).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
			}
			catch (DataException /* dex */)
			{
				// Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return View(student);
		}

		// GET: Student/Delete/5
		public ActionResult Delete(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewBag.ErrorMessage = "Delete action failed. Try again, and if the problem persists SEE your admin sys ADMIN";
			}
			Student student = db.Students.Find(id);
			if (student == null)
			{
				return HttpNotFound();
			}
			return View(student);
		}

		// POST: Student/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id)
		{
			try
			{
				/**
				 * ALTERNATIVE
				 * If improving performance in a high volume application is a priority, 
				 * you could avoid an unnecessary SQL query
				 * **/
				// Student studentToDelete = new Student() { ID = id };
				// db.Entry(studentToDelete).State = EntityState.Deleted;
				// db.SaveChanges();
				Student student = db.Students.Find(id);
				db.Students.Remove(student);
				db.SaveChanges();
			}
			catch (DataException /* dex */)
			{
				// LOG the error
				return RedirectToAction("Delete", new
				{
					id = id,
					saveChangesError = true
				});
			}
			return RedirectToAction("Index");
		}

		// Ensuring that Database Connections Are Not Left Open
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
