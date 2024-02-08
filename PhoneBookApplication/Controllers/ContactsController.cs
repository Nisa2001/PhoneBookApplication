using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneBookApplication.Data;
using PhoneBookApplication.Models;

namespace PhoneBookApplication.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _context;


        //constructor oluşturduk
        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index(string searchName, string searchSurname)
        {
            
            //Linq sorgusu

            var contacts = from m in _context.Contacts
                         select m;

            if (!String.IsNullOrEmpty(searchName))
            {
                contacts = contacts.Where(s => s.Name!.Contains(searchName)); //name içinde searchname değerini konrtol eder
            }

            if (!String.IsNullOrEmpty(searchSurname))
            {
                contacts = contacts.Where(s => s.Surname!.Contains(searchSurname));
            }

            return View(await contacts.ToListAsync());




        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound(); //sayfa bulunamadı
            }

            var contact = await _context.Contacts //contact tablosundan belirli bir kişi alır ve ilk eşleşen kaydı getirir
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact); //contact değişkeni içeren görünümü verir
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        

        [HttpPost] //formdan gelen bilgiler
        [ValidateAntiForgeryToken] //formdan gelen verilerin gerçekliğini doğrular
        public async Task<IActionResult> Create([Bind("Name,Surname,Email,PhoneNumber,Titles,Department")] Contact contact)
        {
           //Contact nesnesi Bind attributeü ile belirtilen özelliklere bağlanır
            
                _context.Add(contact); //contact nesnesine yeni veri eklenir
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id) // Edit işlemi için Controller tarafındaki bir action'ı temsil ediyor
        {
            if (id == null || _context.Contacts == null)
            { 
                //id’yi kontrol eder, context veri bağlantısını sağlıyor. http get isteği gelince sayfa görüntüler
                
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id); //FindAsync(id) metodu ile id ile eşleşen kişiyi arar
            if (contact == null)
            {
                return NotFound(); 
                                   //id ile eşleşen bir kişi bulunursa, bu kişiye ait verileri içeren bir görünümü(View) döndürür.
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //formdan gelen bilgiler
        [ValidateAntiForgeryToken] //formdan gelen verilerin gerçekliğini doğrular
        public async Task<IActionResult> Edit(int id, [Bind("Name,Surname,Email,PhoneNumber,Titles, Department")] Contact contact)
        { //id, düzenlenen kişinin kimliğini temsil eder. contact ise düzenlenmiş kişinin yeni verilerini içerir.

            try
                {
                    contact.ContactId = id; 
                    _context.Update(contact); //nesne güncellenir
                await _context.SaveChangesAsync(); //veri tabanına kaydedilir
            }
                catch (DbUpdateConcurrencyException) //başka kullanıcı güncelleme yapmış mı bakılır
            {
                    if (!ContactExists(contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); //kullanıcıyı index sayfasına yönlendirir

            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id) //bir iletişim bilgisini silmek için çağrılan asenkron bir kontrolör eylemidir
        {
            if (id == null || _context.Contacts == null) //siilinecek kişinin id'si null mı kontrol edilir
            {           
                return NotFound();
            }

            var contact = await _context.Contacts         //belirtilen id değerine sahip iletişim bilgisini veritabanından çekmeye çalışır.                                                               //await, asenkron bir işlem olduğunu belirtir v                                                               //bu işlem tamamlanana kadar beklenir.
           .FirstOrDefaultAsync(m => m.ContactId == id);  //await asenkron oluğunu gösterir
            if (contact == null)                           //bu işlem tamamlanana kadar beklenir. view'e aktarılır
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) //kişiyi silme işlemini gerçekleştirir
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }
            var contact = await _context.Contacts.FindAsync(id);  //id ile eşleşen kişiyi bulmaya çalışır
                                                                 
                                                                 
            if (contact != null)
            {
                _context.Contacts.Remove(contact); //kişiyi siler
            }
            
            await _context.SaveChangesAsync();//veri tabanına kaydeder
            return RedirectToAction(nameof(Index)); //kullanıcyı sayfaya yönlendirir
        }

        private bool ContactExists(int id)  // belirtilen id ile eşleşen bir kişinin var olup olmadığını
                                            // kontrol eder ve bu bilgiyi bool bir değer olarak döndürür.
        {
          return (_context.Contacts?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
