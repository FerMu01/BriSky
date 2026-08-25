document.addEventListener("DOMContentLoaded", function() {
    const sidebarCollapse = document.getElementById('sidebarCollapse');
    const sidebar = document.getElementById('sidebar');

    if (sidebarCollapse && sidebar) {
        sidebarCollapse.addEventListener('click', function() {
            sidebar.classList.toggle('active');
            
            if (sidebar.classList.contains('active') && window.innerWidth > 768) {
                document.querySelectorAll('#sidebar .collapse').forEach(function(el) {
                    el.classList.remove('show');
                    el.previousElementSibling.setAttribute('aria-expanded', 'false');
                    el.style.display = 'none';
                });
            }
        });
    }

    const dropdownToggles = document.querySelectorAll('.dropdown-toggle');
    dropdownToggles.forEach(function(toggle) {
        toggle.addEventListener('click', function(e) {
            e.preventDefault();
            
            if (sidebar.classList.contains('active') && window.innerWidth > 768) {
                return;
            }

            const targetId = this.getAttribute('href').substring(1);
            const targetMenu = document.getElementById(targetId);
            
            const isExpanded = this.getAttribute('aria-expanded') === 'true';

            if (isExpanded) {
                this.setAttribute('aria-expanded', 'false');
                targetMenu.classList.remove('show');
                targetMenu.style.display = 'none';
            } else {
                this.setAttribute('aria-expanded', 'true');
                targetMenu.classList.add('show');
                targetMenu.style.display = 'block';
            }
        });
    });
    
    const links = document.querySelectorAll('#sidebar ul li a:not(.dropdown-toggle)');
    
    links.forEach(function(link) {
        const linkHref = link.getAttribute('href');
        if (!linkHref || linkHref.startsWith('#')) return;
        
        let urlToCheck = link.href.toLowerCase();
        
        const normalize = (url) => url.replace(/\/$/, '').replace(/\/default\.aspx$/, '');
        
        if (normalize(urlToCheck) === normalize(window.location.href.toLowerCase())) {
            link.parentElement.classList.add('active');
            
            const parentMenu = link.closest('.collapse');
            if (parentMenu) {
                parentMenu.classList.add('show');
                parentMenu.style.display = 'block';
                parentMenu.previousElementSibling.setAttribute('aria-expanded', 'true');
                parentMenu.closest('li.has-submenu').classList.add('active');
            }
        }
    });

    document.querySelectorAll('#sidebar .collapse:not(.show)').forEach(function(el) {
        el.style.display = 'none';
    });
});
