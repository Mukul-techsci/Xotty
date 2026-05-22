// ==================== DOCUMENT READY ====================
$(document).ready(function () {
    initializePortfolio();
});

// ==================== INITIALIZE PORTFOLIO ====================
function initializePortfolio() {
    // Initialize functions in order
    typingAnimation();        // Initialize typing animation
    scrollToTopButton();      // Scroll to top functionality
    darkModeToggle();         // Dark mode toggle
    navbarScroll();           // Navbar scroll effects
    navbarActiveLink();       // Active navbar link
    smoothScrolling();        // Smooth page scrolling

    // Initialize AOS after a small delay
    setTimeout(function () {
        aosInitialize();
    }, 500);

    contactFormHandler();     // Contact form submission
    loadingAnimation();       // Loading animations
}

// ==================== TYPING ANIMATION (IMPROVED) ====================
function typingAnimation() {
    // Wait for Typed.js library to load
    let attempts = 0;
    const maxAttempts = 10;

    const initTyped = setInterval(function () {
        const typedElement = document.getElementById('typedText');

        if (typeof Typed !== 'undefined' && typedElement) {
            clearInterval(initTyped);

            try {
                new Typed('#typedText', {
                    strings: [
                        'ASP.NET MVC Developer',
                        '.NET Core Developer',
                        'Full Stack Engineer',
                        'SQL Server Expert',
                        'C# Developer',
                        'Backend Developer',
                        'Frontend Developer',
                        'Web Application Developer',
                        'Software Engineer',
                        'API Developer',
                        'REST API Specialist',
                        'Database Developer',
                        'Microsoft Technologies Expert',
                        'Performance Optimization Expert',
                        'Azure Developer',
                        'Dynamic Web Solutions Architect'
                    ],
                    typeSpeed: 50,
                    backSpeed: 30,
                    backDelay: 1500,
                    loop: true,
                    cursorChar: '|',
                    showCursor: true,
                    startDelay: 300
                });

                console.log('Typed.js initialized successfully');
            } catch (error) {
                console.error('Error initializing Typed.js:', error);
                // Fallback: Just display static text
                typedElement.textContent = 'ASP.NET MVC Developer';
            }
        } else if (attempts >= maxAttempts) {
            clearInterval(initTyped);
            console.warn('Typed.js not loaded after max attempts');
            const typedElement = document.getElementById('typedText');
            if (typedElement) {
                typedElement.textContent = 'ASP.NET MVC Developer';
            }
        }

        attempts++;
    }, 300);
}

// ==================== SCROLL TO TOP BUTTON ====================
function scrollToTopButton() {
    const scrollBtn = $('#scrollToTop');

    // Show/hide button based on scroll
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            scrollBtn.addClass('visible');
        } else {
            scrollBtn.removeClass('visible');
        }
    });

    // Click handler
    scrollBtn.click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 600, 'swing');
    });
}

// ==================== DARK MODE TOGGLE ====================
function darkModeToggle() {
    const darkModeBtn = $('#darkModeToggle');
    const isDarkMode = localStorage.getItem('darkMode') === 'true';

    // Set initial theme
    if (isDarkMode) {
        $('body').addClass('dark-mode');
        darkModeBtn.html('<i class="fas fa-sun"></i>');
    } else {
        $('body').removeClass('dark-mode');
        darkModeBtn.html('<i class="fas fa-moon"></i>');
    }

    // Toggle on button click
    darkModeBtn.click(function () {
        $('body').toggleClass('dark-mode');
        const isDark = $('body').hasClass('dark-mode');
        localStorage.setItem('darkMode', isDark);

        if (isDark) {
            darkModeBtn.html('<i class="fas fa-sun"></i>');
        } else {
            darkModeBtn.html('<i class="fas fa-moon"></i>');
        }
    });
}

// ==================== NAVBAR SCROLL EFFECT ====================
function navbarScroll() {
    const navbar = $('.navbar-custom');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            navbar.addClass('scrolled');
        } else {
            navbar.removeClass('scrolled');
        }
    });
}

// ==================== NAVBAR ACTIVE LINK - FIXED ====================
function navbarActiveLink() {
    // Get all navbar links
    const navLinks = $('.navbar-nav .nav-link');
    const navbarCollapse = $('.navbar-collapse');
    const navbarToggler = $('.navbar-toggler');

    // Handle link clicks
    navLinks.on('click', function (e) {
        const href = $(this).attr('href');

        // Check if it's an anchor link
        if (href && href.startsWith('#') && href !== '#') {
            e.preventDefault();

            // Get target section
            const targetSection = $(href);

            if (targetSection.length) {
                // Remove active from all links
                navLinks.removeClass('active');

                // Add active to clicked link
                $(this).addClass('active');

                // Smooth scroll to section
                $('html, body').animate({
                    scrollTop: targetSection.offset().top - 80
                }, 600, 'swing');

                // Close mobile menu if open
                if (navbarCollapse.hasClass('show')) {
                    navbarToggler.click();
                }
            }
        }
    });

    // Update active link on page scroll
    $(window).on('scroll', function () {
        const scrollPos = $(window).scrollTop() + 100;

        // Check each section
        $('section[id]').each(function () {
            const sectionTop = $(this).offset().top;
            const sectionBottom = sectionTop + $(this).height();
            const sectionId = $(this).attr('id');

            if (scrollPos >= sectionTop && scrollPos < sectionBottom) {
                // Remove active from all
                navLinks.removeClass('active');

                // Add active to matching link
                $(`.navbar-nav a[href="#${sectionId}"]`).addClass('active');
            }
        });
    });
}

// ==================== SMOOTH SCROLLING ====================
function smoothScrolling() {
    // Already handled in navbarActiveLink
    // This ensures all anchor links work smoothly
    $(document).on('click', 'a[href^="#"]', function (e) {
        const href = $(this).attr('href');

        // Skip if it's just '#'
        if (href === '#') {
            e.preventDefault();
            return;
        }

        const target = $(href);

        if (target.length && !$(this).hasClass('nav-link')) {
            e.preventDefault();

            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 600, 'swing');
        }
    });
}

// ==================== AOS INITIALIZE ====================
function aosInitialize() {
    if (typeof AOS !== 'undefined') {
        AOS.init({
            duration: 800,
            easing: 'ease-in-out',
            once: true,
            offset: 100,
            delay: 100
        });

        console.log('AOS initialized successfully');
    } else {
        console.warn('AOS library not loaded');
    }
}

// ==================== CONTACT FORM HANDLER ====================
function contactFormHandler() {
    const contactForm = $('#contactForm');
    const successMessage = $('#successMessage');
    const errorMessage = $('#errorMessage');
    const submitBtn = $('#submitBtn');

    if (contactForm.length > 0) {
        contactForm.on('submit', function (e) {
            e.preventDefault();

            // Validate form
            if (!this.checkValidity() === false) {
                // Show loading state
                const originalBtnText = submitBtn.html();
                submitBtn.html('<i class="fas fa-spinner fa-spin"></i> Sending...').prop('disabled', true);

                // Get form data
                const formData = {
                    Name: $('#contactName').val(),
                    Email: $('#contactEmail').val(),
                    Phone: $('#contactPhone').val(),
                    Subject: $('#contactSubject').val(),
                    Message: $('#contactMessage').val()
                };

                // Send via AJAX
                $.ajax({
                    type: 'POST',
                    url: $(this).attr('action'),
                    data: $(this).serialize(),
                    success: function (response) {
                        // Show success message
                        successMessage.slideDown();
                        errorMessage.slideUp();

                        // Reset form
                        contactForm[0].reset();

                        // Restore button
                        submitBtn.html(originalBtnText).prop('disabled', false);

                        // Hide message after 5 seconds
                        setTimeout(function () {
                            successMessage.slideUp();
                        }, 5000);
                    },
                    error: function (error) {
                        // Show error message
                        errorMessage.slideDown();

                        // Restore button
                        submitBtn.html(originalBtnText).prop('disabled', false);

                        console.error('Form submission error:', error);

                        // Hide message after 5 seconds
                        setTimeout(function () {
                            errorMessage.slideUp();
                        }, 5000);
                    }
                });
            }
        });
    }
}

// ==================== LOADING ANIMATION ====================
function loadingAnimation() {
    if (sessionStorage.getItem('pageLoaded')) {
        return;
    }

    sessionStorage.setItem('pageLoaded', 'true');

    // Animate counters when visible
    animateCounters();
}

// ==================== ANIMATE COUNTERS ====================
function animateCounters() {
    let animated = false;

    $(window).on('scroll', function () {
        if (!animated && isElementInViewport($('.stat-card').first()[0])) {
            $('.stat-number').each(function () {
                const targetText = $(this).text();
                const target = parseFloat(targetText);
                const increment = target / 100;
                let current = 0;

                const timer = setInterval(() => {
                    current += increment;
                    if (current >= target) {
                        $(this).text(targetText);
                        clearInterval(timer);
                    } else {
                        $(this).text(current.toFixed(1));
                    }
                }, 10);
            });

            animated = true;
        }
    });
}

// ==================== IS ELEMENT IN VIEWPORT ====================
function isElementInViewport(el) {
    if (!el) return false;
    const rect = el.getBoundingClientRect();
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

// ==================== PARALLAX EFFECT ====================
function parallaxEffect() {
    $(window).on('scroll', function () {
        const scrollTop = $(this).scrollTop();

        // Hero section parallax
        $('.hero-section').css({
            'background-position': `center ${scrollTop * 0.5}px`
        });
    });
}

// Initialize parallax on load
$(window).on('load', function () {
    parallaxEffect();
});

// ==================== MOBILE MENU CLOSE ====================
function handleMobileMenu() {
    $('.navbar-nav a').on('click', function () {
        if ($(window).width() < 992) {
            $('.navbar-toggler').click();
        }
    });
}

$(document).ready(function () {
    handleMobileMenu();
});

// ==================== PREVENT DEFAULT ACTIONS ====================
$(document).on('click', 'a[href="#"]', function (e) {
    e.preventDefault();
});

// ==================== ERROR HANDLING FOR IMAGES ====================
$(document).on('error', 'img', function () {
    $(this).attr('src', 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="200" height="200"%3E%3Crect width="200" height="200" fill="%23ddd"/%3E%3Ctext x="50%25" y="50%25" dominant-baseline="middle" text-anchor="middle" font-family="Arial" font-size="14" fill="%23999"%3EImage Not Found%3C/text%3E%3C/svg%3E');
    $(this).addClass('error-image');
});

// ==================== ORIENTATION CHANGE ====================
$(window).on('orientationchange', function () {
    if (typeof AOS !== 'undefined') {
        AOS.refresh();
    }
    console.log('Orientation changed');
});

// ==================== READY STATE ====================
$(document).ready(function () {
    // Refresh AOS after a delay
    setTimeout(function () {
        if (typeof AOS !== 'undefined') {
            AOS.refresh();
        }
    }, 800);
});

console.log('Portfolio JavaScript loaded successfully');