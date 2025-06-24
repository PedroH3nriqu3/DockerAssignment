// Configuração da API
const API_BASE_URL = 'http://localhost:5144';

// Função para mostrar mensagens
function showMessage(message, type = 'info') {
    const messageDiv = document.getElementById('message');
    messageDiv.innerHTML = `<div class="message ${type}">${message}</div>`;
    
    // Auto-remover mensagem após 5 segundos
    setTimeout(() => {
        messageDiv.innerHTML = '';
    }, 5000);
}

// Função para alternar entre abas
function showTab(event, tabName) {
    // Atualizar abas
    document.querySelectorAll('.tab').forEach(tab => {
        tab.classList.remove('active');
    });
    event.target.classList.add('active');
    
    // Atualizar formulários
    document.querySelectorAll('.form-container').forEach(form => {
        form.classList.remove('active');
    });
    
    if (tabName === 'login') {
        document.getElementById('login-form').classList.add('active');
    } else {
        document.getElementById('register-form').classList.add('active');
    }
    
    // Limpar mensagens
    document.getElementById('message').innerHTML = '';
}

// Função para fazer login
async function login() {
    const username = document.getElementById('login-username').value.trim();
    const password = document.getElementById('login-password').value.trim();
    
    // Validação básica
    if (!username || !password) {
        showMessage('Por favor, preencha todos os campos.', 'error');
        return;
    }
    
    try {
        showMessage('Fazendo login...', 'info');
        
        const response = await fetch(`${API_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        });
        
        if (response.ok) {
            showMessage('✅ Login realizado com sucesso!', 'success');
            // Limpar campos após login bem-sucedido
            document.getElementById('login-username').value = '';
            document.getElementById('login-password').value = '';
        } else if (response.status === 401) {
            showMessage('❌ Usuário ou senha incorretos.', 'error');
        } else {
            showMessage(`❌ Erro no servidor: ${response.status}`, 'error');
        }
        
    } catch (error) {
        console.error('Erro na requisição:', error);
        showMessage('❌ Erro de conexão. Verifique se o servidor está rodando.', 'error');
    }
}

// Função para fazer registro
async function register() {
    const username = document.getElementById('register-username').value.trim();
    const password = document.getElementById('register-password').value.trim();
    
    // Validação básica
    if (!username || !password) {
        showMessage('Por favor, preencha todos os campos.', 'error');
        return;
    }
    
    if (password.length < 3) {
        showMessage('A senha deve ter pelo menos 3 caracteres.', 'error');
        return;
    }
    
    try {
        showMessage('Registrando usuário...', 'info');
        
        const response = await fetch(`${API_BASE_URL}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        });
        
        if (response.ok) {
            showMessage('✅ Usuário registrado com sucesso!', 'success');
            // Limpar campos após registro bem-sucedido
            document.getElementById('register-username').value = '';
            document.getElementById('register-password').value = '';
        } else {
            showMessage(`❌ Erro no registro: ${response.status}`, 'error');
        }
        
    } catch (error) {
        console.error('Erro na requisição:', error);
        showMessage('❌ Erro de conexão. Verifique se o servidor está rodando.', 'error');
    }
}

// Função para permitir login com Enter
function handleKeyPress(event, action) {
    if (event.key === 'Enter') {
        if (action === 'login') {
            login();
        } else if (action === 'register') {
            register();
        }
    }
}

// Adicionar event listeners para Enter
document.addEventListener('DOMContentLoaded', function() {
    // Login form
    document.getElementById('login-username').addEventListener('keypress', (e) => handleKeyPress(e, 'login'));
    document.getElementById('login-password').addEventListener('keypress', (e) => handleKeyPress(e, 'login'));
    
    // Register form
    document.getElementById('register-username').addEventListener('keypress', (e) => handleKeyPress(e, 'register'));
    document.getElementById('register-password').addEventListener('keypress', (e) => handleKeyPress(e, 'register'));
    
    // Mostrar mensagem inicial
    showMessage('🚀 Bem-vindo! Conecte-se ao servidor de autenticação.', 'info');
});