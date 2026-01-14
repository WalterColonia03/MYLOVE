/* ===============================================
   SAVE ME: THE BTS MYSTERY GAME
   Hidden Object Game Engine
   =============================================== */

// Configuración de los escenarios del juego
const hogEscenarios = [
    {
        id: 'jungkook',
        miembro: 'Jungkook',
        fotoMiembro: './assets/img/7_Jungkook.png',
        fondo: './assets/img/hog/scene_jungkook.png',
        audio: './assets/audio/BTS  ON [Sub. Español].mp3',
        segundoInicio: 26,
        objetos: [
            {
                id: 'pajaro',
                nombre: 'Dibujo de pájaro',
                imagen: './assets/img/hog/objects/object_bird_sketch_1768363605239.png',
                x: 15, y: 55, // Porcentaje de posición
                width: 8, height: 10,
                pista: '🐦 Un ave que sueña con volar libre...'
            }
        ],
        mensajeIntro: 'Jungkook tuvo un accidente. Encuentra el dibujo que revela sus sueños.',
        contexto: 'El menor del grupo siempre soñó con volar como un pájaro...'
    },
    {
        id: 'suga',
        miembro: 'Suga',
        fotoMiembro: './assets/img/3_SUGA.png',
        fondo: './assets/img/hog/scene_suga.png',
        audio: './assets/audio/DDAENG (땡)  RM, SUGA, J-HOPE (BTS) [Traducida al Español].mp3',
        segundoInicio: 124, // 2:04
        objetos: [
            {
                id: 'encendedor',
                nombre: 'Encendedor blanco',
                imagen: './assets/img/hog/objects/object_white_lighter_1768363620082.png',
                x: 25, y: 68,
                width: 6, height: 8,
                pista: '🔥 Una pequeña llama que puede destruir todo...'
            }
        ],
        mensajeIntro: 'La habitación de Suga guarda un secreto oscuro. Encuentra lo que inició todo.',
        contexto: 'En el motel, Yoongi luchaba contra sus demonios internos...'
    },
    {
        id: 'jhope',
        miembro: 'J-Hope',
        fotoMiembro: './assets/img/4_J-Hope.png',
        fondo: './assets/img/hog/scene_jhope.png',
        audio: './assets/audio/j-hope - Killin\' It Girl (Solo Version) (sub español).mp3',
        segundoInicio: 2,
        objetos: [
            {
                id: 'pastillas',
                nombre: 'Frasco de pastillas',
                imagen: './assets/img/hog/objects/object_pill_bottle_1768363636210.png',
                x: 20, y: 75,
                width: 7, height: 9,
                pista: '💊 Medicina que promete aliviar el dolor...'
            }
        ],
        mensajeIntro: 'En el puente, J-Hope espera. Encuentra lo que le mantiene despierto.',
        contexto: 'Hoseok sufría de una condición que le hacía colapsar sin aviso...'
    },
    {
        id: 'rm',
        miembro: 'RM',
        fotoMiembro: './assets/img/1_RM.png',
        fondo: './assets/img/hog/scene_rm.png',
        audio: './assets/audio/MIC Drop.mp3',
        segundoInicio: 171, // 2:51
        objetos: [
            {
                id: 'espejo',
                nombre: 'Espejo con garabatos',
                imagen: './assets/img/hog/objects/object_cracked_mirror_1768363651593.png',
                x: 8, y: 35,
                width: 12, height: 15,
                pista: '🪞 Un reflejo roto que muestra la verdad...'
            }
        ],
        mensajeIntro: 'En la gasolinera abandonada, RM busca respuestas. Encuentra su reflejo.',
        contexto: 'Namjoon trabajaba aquí para sobrevivir, escribiendo sus sueños en el espejo...'
    },
    {
        id: 'jimin',
        miembro: 'Jimin',
        fotoMiembro: './assets/img/5_jimin.png',
        fondo: './assets/img/hog/scene_jimin.png',
        audio: './assets/audio/Magic Shop.mp3',
        segundoInicio: 65, // 1:05
        objetos: [
            {
                id: 'manzana',
                nombre: 'Manzana roja',
                imagen: './assets/img/hog/objects/object_red_apple_1768363680547.png',
                x: 45, y: 40,
                width: 6, height: 7,
                pista: '🍎 Una fruta que flota en aguas de soledad...'
            }
        ],
        mensajeIntro: 'El hospital guarda los secretos de Jimin. Encuentra lo que flota en el agua.',
        contexto: 'En la soledad del hospital, Jimin buscaba escapar de sí mismo...'
    },
    {
        id: 'v',
        miembro: 'V',
        fotoMiembro: './assets/img/6_V.png',
        fondo: './assets/img/hog/scene_v.png',
        audio: './assets/audio/JUMP.mp3',
        segundoInicio: 98, // 1:38
        objetos: [
            {
                id: 'spray',
                nombre: 'Lata de spray',
                imagen: './assets/img/hog/objects/object_spray_can_1768363694230.png',
                x: 55, y: 72,
                width: 7, height: 10,
                pista: '🎨 Un instrumento de rebeldía y expresión...'
            }
        ],
        mensajeIntro: 'En el callejón, V dejó su marca. Encuentra su herramienta de expresión.',
        contexto: 'Taehyung expresaba su dolor familiar a través del arte callejero...'
    },
    {
        id: 'jin',
        miembro: 'Jin',
        fotoMiembro: './assets/img/2_JIN.png',
        fondo: './assets/img/hog/scene_jin.png',
        audio: './assets/audio/Just One Day.mp3',
        segundoInicio: 170, // 2:50
        objetos: [
            {
                id: 'smeraldo',
                nombre: 'Flor Smeraldo',
                imagen: './assets/img/hog/objects/object_smeraldo_1768363708270.png',
                x: 62, y: 32,
                width: 10, height: 12,
                pista: '🌸 La flor mágica que representa la verdad no dicha...'
            },
            {
                id: 'camara',
                nombre: 'Cámara Polaroid',
                imagen: './assets/img/hog/objects/object_polaroid_camera_1768363723974.png',
                x: 22, y: 55,
                width: 9, height: 10,
                pista: '📸 Captura momentos que el tiempo no puede cambiar...'
            }
        ],
        mensajeIntro: 'Jin ha viajado en el tiempo muchas veces. Encuentra los objetos que romperán el bucle.',
        contexto: 'Seokjin es el viajero del tiempo, intentando salvar a sus amigos una y otra vez...'
    }
];

// Estado del juego
let hogState = {
    escenarioActual: 0,
    objetosEncontrados: [],
    juegoActivo: false,
    timerPista: null,
    audioActual: null
};

// Inicializar el juego HOG
function iniciarJuegoHOG() {
    hogState = {
        escenarioActual: 0,
        objetosEncontrados: [],
        juegoActivo: true,
        timerPista: null,
        audioActual: null
    };

    // Mostrar intro de Jin (la llamada)
    mostrarIntroHOG();
}

// Mostrar la introducción del juego (llamada de Jin)
function mostrarIntroHOG() {
    const introHTML = `
        <div class="hog-intro-overlay">
            <div class="hog-phone-container">
                <div class="phone-ring-animation">📞</div>
                <div class="jin-photo">
                    <img src="./assets/img/2_JIN.png" alt="Jin" class="jin-avatar">
                </div>
                <div class="jin-message-container">
                    <p class="jin-name">Jin está llamando...</p>
                    <p class="jin-message typewriter"></p>
                </div>
                <button class="btn-answer-call" onclick="contestarLlamada()">
                    <span class="call-icon">📱</span>
                    Contestar
                </button>
            </div>
        </div>
    `;

    document.getElementById('pantalla-hog').innerHTML = introHTML;

    // Animación de tipeo del mensaje de Jin
    setTimeout(() => {
        const mensaje = "Hola... Soy Jin. He intentado salvarlos muchas veces, pero necesito tus ojos para encontrar las pistas que me faltan. ¿Me ayudas a arreglar el destino?";
        typewriterEffect(document.querySelector('.jin-message'), mensaje, 40);
    }, 1000);
}

// Efecto de máquina de escribir
function typewriterEffect(element, text, speed) {
    let i = 0;
    element.innerHTML = '';

    function type() {
        if (i < text.length) {
            element.innerHTML += text.charAt(i);
            i++;
            setTimeout(type, speed);
        }
    }
    type();
}

// Contestar la llamada y empezar el juego
function contestarLlamada() {
    const intro = document.querySelector('.hog-intro-overlay');
    intro.classList.add('fade-out');

    setTimeout(() => {
        cargarEscenario(0);
    }, 500);
}

// Cargar un escenario del juego
function cargarEscenario(indice) {
    if (indice >= hogEscenarios.length) {
        // Juego completado - transición espectacular
        mostrarTransicionFinal();
        return;
    }

    hogState.escenarioActual = indice;
    hogState.objetosEncontrados = [];

    const escenario = hogEscenarios[indice];

    // Crear HTML del escenario
    const escenarioHTML = `
        <div class="hog-scene-container">
            <!-- Fondo del escenario -->
            <div class="hog-background">
                <img src="${escenario.fondo}" alt="${escenario.miembro}" class="scene-bg-img">
                
                <!-- Capa de objetos clickeables -->
                <div class="hog-objects-layer" id="objectsLayer">
                    ${escenario.objetos.map(obj => `
                        <div class="hog-object" 
                             id="obj-${obj.id}"
                             data-id="${obj.id}"
                             style="left: ${obj.x}%; top: ${obj.y}%; width: ${obj.width}%; height: ${obj.height}%;"
                             onclick="clickObjeto('${obj.id}')">
                            <img src="${obj.imagen}" alt="${obj.nombre}">
                        </div>
                    `).join('')}
                </div>
            </div>
            
            <!-- Panel de información -->
            <div class="hog-info-panel">
                <div class="hog-member-info">
                    <div class="member-avatar">
                        <img src="${escenario.fotoMiembro}" alt="${escenario.miembro}">
                    </div>
                    <div class="member-details">
                        <h3 class="member-name">${escenario.miembro}</h3>
                        <p class="case-number">Caso ${indice + 1} de 7</p>
                    </div>
                </div>
                
                <div class="hog-hint-box">
                    <p class="hint-label">🔍 Pista:</p>
                    <p class="hint-text" id="currentHint">${escenario.objetos[0].pista}</p>
                </div>
                
                <div class="hog-progress">
                    <div class="progress-label">Objetos: <span id="objCount">0</span>/${escenario.objetos.length}</div>
                    <div class="progress-bar-hog">
                        <div class="progress-fill-hog" id="progressFill" style="width: 0%"></div>
                    </div>
                </div>
                
                <button class="btn-reveal" onclick="revelarObjetos()">
                    <span class="reveal-icon">✨</span>
                    Revelar
                </button>
            </div>
            
            <!-- Mensaje de contexto -->
            <div class="hog-context-message" id="contextMessage">
                <p>${escenario.mensajeIntro}</p>
            </div>
        </div>
    `;

    document.getElementById('pantalla-hog').innerHTML = escenarioHTML;

    // Iniciar música del escenario
    cambiarMusicaHOG(escenario.audio, escenario.segundoInicio);

    // Iniciar timer de pista (brillo después de 20s)
    iniciarTimerPista();

    // Ocultar mensaje de contexto después de 4 segundos
    setTimeout(() => {
        const msg = document.getElementById('contextMessage');
        if (msg) msg.classList.add('fade-out');
    }, 4000);
}

// Click en un objeto
function clickObjeto(objetoId) {
    if (hogState.objetosEncontrados.includes(objetoId)) return;

    const escenario = hogEscenarios[hogState.escenarioActual];
    const objeto = escenario.objetos.find(o => o.id === objetoId);

    if (!objeto) return;

    // Marcar como encontrado
    hogState.objetosEncontrados.push(objetoId);

    // Animación de encontrado
    const objetoElement = document.getElementById(`obj-${objetoId}`);
    objetoElement.classList.add('found');

    // Crear efecto de partículas
    crearParticulasExito(objetoElement);

    // Reproducir sonido de encontrado
    reproducirSonidoEncontrado();

    // Actualizar progreso
    actualizarProgresoHOG();

    // Reiniciar timer de pista
    clearTimeout(hogState.timerPista);
    iniciarTimerPista();

    // Verificar si se completó el escenario
    if (hogState.objetosEncontrados.length >= escenario.objetos.length) {
        setTimeout(() => {
            completarEscenario();
        }, 1500);
    } else {
        // Mostrar siguiente pista
        const siguienteObjeto = escenario.objetos.find(o => !hogState.objetosEncontrados.includes(o.id));
        if (siguienteObjeto) {
            document.getElementById('currentHint').textContent = siguienteObjeto.pista;
        }
    }
}

// Crear partículas de éxito al encontrar objeto
function crearParticulasExito(elemento) {
    const rect = elemento.getBoundingClientRect();
    const centerX = rect.left + rect.width / 2;
    const centerY = rect.top + rect.height / 2;

    for (let i = 0; i < 15; i++) {
        const particula = document.createElement('div');
        particula.className = 'success-particle';
        particula.innerHTML = ['✨', '💜', '⭐', '💫'][Math.floor(Math.random() * 4)];
        particula.style.left = centerX + 'px';
        particula.style.top = centerY + 'px';
        particula.style.setProperty('--tx', (Math.random() - 0.5) * 200 + 'px');
        particula.style.setProperty('--ty', (Math.random() - 0.5) * 200 + 'px');
        document.body.appendChild(particula);

        setTimeout(() => particula.remove(), 1000);
    }
}

// Reproducir sonido de encontrado
function reproducirSonidoEncontrado() {
    // Crear un sonido simple usando Web Audio API
    const audioContext = new (window.AudioContext || window.webkitAudioContext)();
    const oscillator = audioContext.createOscillator();
    const gainNode = audioContext.createGain();

    oscillator.connect(gainNode);
    gainNode.connect(audioContext.destination);

    oscillator.frequency.setValueAtTime(800, audioContext.currentTime);
    oscillator.frequency.exponentialRampToValueAtTime(1200, audioContext.currentTime + 0.1);

    gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
    gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.3);

    oscillator.start(audioContext.currentTime);
    oscillator.stop(audioContext.currentTime + 0.3);
}

// Actualizar barra de progreso
function actualizarProgresoHOG() {
    const escenario = hogEscenarios[hogState.escenarioActual];
    const total = escenario.objetos.length;
    const encontrados = hogState.objetosEncontrados.length;
    const porcentaje = (encontrados / total) * 100;

    document.getElementById('objCount').textContent = encontrados;
    document.getElementById('progressFill').style.width = porcentaje + '%';
}

// Revelar objetos (autocompletar)
function revelarObjetos() {
    const escenario = hogEscenarios[hogState.escenarioActual];

    // Mostrar todos los objetos no encontrados con animación
    escenario.objetos.forEach((obj, index) => {
        if (!hogState.objetosEncontrados.includes(obj.id)) {
            setTimeout(() => {
                const objetoElement = document.getElementById(`obj-${obj.id}`);
                if (objetoElement) {
                    objetoElement.classList.add('revealed');

                    // Simular click después de la animación
                    setTimeout(() => {
                        clickObjeto(obj.id);
                    }, 800);
                }
            }, index * 600);
        }
    });
}

// Iniciar timer de pista (brillo después de 20s de inactividad)
function iniciarTimerPista() {
    clearTimeout(hogState.timerPista);

    hogState.timerPista = setTimeout(() => {
        const escenario = hogEscenarios[hogState.escenarioActual];
        const objetoNoEncontrado = escenario.objetos.find(o => !hogState.objetosEncontrados.includes(o.id));

        if (objetoNoEncontrado) {
            const elemento = document.getElementById(`obj-${objetoNoEncontrado.id}`);
            if (elemento) {
                elemento.classList.add('hint-glow');

                // Quitar el brillo después de 3 segundos
                setTimeout(() => {
                    elemento.classList.remove('hint-glow');
                    iniciarTimerPista(); // Reiniciar timer
                }, 3000);
            }
        }
    }, 20000); // 20 segundos
}

// Completar escenario actual
function completarEscenario() {
    const escenario = hogEscenarios[hogState.escenarioActual];

    // Mostrar mensaje de completado
    const completadoHTML = `
        <div class="hog-completed-overlay">
            <div class="completed-content">
                <div class="completed-stamp">✅ CASO RESUELTO</div>
                <img src="${escenario.fotoMiembro}" alt="${escenario.miembro}" class="completed-member">
                <h2>${escenario.miembro} está a salvo</h2>
                <p>${escenario.contexto}</p>
                <button class="btn-next-case" onclick="siguienteEscenario()">
                    ${hogState.escenarioActual < hogEscenarios.length - 1 ? 'Siguiente Caso →' : 'Finalizar ✨'}
                </button>
            </div>
        </div>
    `;

    document.getElementById('pantalla-hog').insertAdjacentHTML('beforeend', completadoHTML);
}

// Ir al siguiente escenario
function siguienteEscenario() {
    cargarEscenario(hogState.escenarioActual + 1);
}

// Cambiar música del juego
function cambiarMusicaHOG(ruta, segundoInicio) {
    const audio = document.getElementById('musica-fondo');

    // Fade out de la música actual
    if (audio.src && !audio.paused) {
        let volumen = audio.volume;
        const fadeOut = setInterval(() => {
            volumen -= 0.1;
            if (volumen <= 0) {
                clearInterval(fadeOut);
                audio.pause();
                cargarNuevaMusica(ruta, segundoInicio);
            } else {
                audio.volume = volumen;
            }
        }, 50);
    } else {
        cargarNuevaMusica(ruta, segundoInicio);
    }
}

function cargarNuevaMusica(ruta, segundoInicio) {
    const audio = document.getElementById('musica-fondo');
    audio.src = ruta;
    audio.currentTime = segundoInicio;
    audio.volume = 0;
    audio.play().catch(e => console.log('Audio autoplay blocked'));

    // Fade in
    let volumen = 0;
    const fadeIn = setInterval(() => {
        volumen += 0.05;
        if (volumen >= 0.7) {
            clearInterval(fadeIn);
            audio.volume = 0.7;
        } else {
            audio.volume = volumen;
        }
    }, 50);
}

// Mostrar transición final espectacular
function mostrarTransicionFinal() {
    hogState.juegoActivo = false;

    const transicionHTML = `
        <div class="hog-final-transition">
            <div class="transition-particles" id="transitionParticles"></div>
            <div class="transition-content">
                <div class="smeraldo-reveal">
                    <img src="./assets/img/hog/objects/object_smeraldo_1768363708270.png" alt="Smeraldo" class="smeraldo-final">
                </div>
                <div class="transition-text">
                    <h2 class="transition-title">✨ El destino ha cambiado ✨</h2>
                    <p class="transition-subtitle">Todos están a salvo...</p>
                </div>
            </div>
        </div>
    `;

    document.getElementById('pantalla-hog').innerHTML = transicionHTML;

    // Crear partículas de transición
    crearParticulasTransicion();

    // Después de 4 segundos, ir a la pantalla final
    setTimeout(() => {
        document.getElementById('pantalla-hog').classList.add('hidden');
        document.getElementById('pantalla-hog').classList.remove('activa');
        mostrarFinal();
    }, 5000);
}

// Crear partículas para la transición final
function crearParticulasTransicion() {
    const container = document.getElementById('transitionParticles');
    const emojis = ['💜', '✨', '🌸', '💫', '⭐', '🦋', '💕'];

    for (let i = 0; i < 50; i++) {
        const particula = document.createElement('div');
        particula.className = 'transition-particle';
        particula.innerHTML = emojis[Math.floor(Math.random() * emojis.length)];
        particula.style.left = Math.random() * 100 + '%';
        particula.style.animationDelay = Math.random() * 3 + 's';
        particula.style.animationDuration = (3 + Math.random() * 2) + 's';
        container.appendChild(particula);
    }
}
